using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;
using Bolsover.Converter;
using Bolsover.Import;

namespace Bolsover.Splitterator
{
    public abstract class StlSplitterator
    {
        /// <summary>
        ///     Synchronous STL parser that delegates to the async implementation.
        ///     Prefer using <see cref="ParseStlAsync" /> in new code for non-blocking I/O.
        /// </summary>
        public static List<Triangle> ParseStl(string path)
        {
            // Delegate to async version to ensure a single I/O implementation path.
            return ParseStlAsync(path).GetAwaiter().GetResult();
        }

        /// <summary>
        ///     Asynchronously reads an STL file using <see cref="StlReader.ReadStlAsync" /> and converts the
        ///     returned node list (x, y, z flattened) into a list of <see cref="Triangle" /> objects.
        /// </summary>
        /// <param name="path">Path to the STL file.</param>
        /// <param name="token">Cancellation token.</param>
        /// <param name="progress">Optional textual progress reporter.</param>
        /// <returns>List of triangles parsed from the STL file.</returns>
        private static async Task<List<Triangle>> ParseStlAsync(string path, CancellationToken token = default,
            IProgress<string> progress = null)
        {
            var nodes = await StlReader.ReadStlAsync(path, token, progress).ConfigureAwait(false);
            return ConvertNodesToTriangles(nodes);
        }

        /// <summary>
        ///     Converts a flattened list of doubles [x0,y0,z0, x1,y1,z1, x2,y2,z2, ...] coming from the
        ///     STL reader into a list of <see cref="Triangle" /> instances.
        /// </summary>
        private static List<Triangle> ConvertNodesToTriangles(List<double> nodes)
        {
            var triangles = new List<Triangle>(nodes == null ? 0 : nodes.Count / 9);
            if (nodes == null || nodes.Count < 9) return triangles;

            for (var i = 0; i + 8 < nodes.Count; i += 9)
            {
                var tri = new Triangle
                {
                    Vertices =
                    {
                        [0] = new Vector3((float)nodes[i], (float)nodes[i + 1], (float)nodes[i + 2]),
                        [1] = new Vector3((float)nodes[i + 3], (float)nodes[i + 4], (float)nodes[i + 5]),
                        [2] = new Vector3((float)nodes[i + 6], (float)nodes[i + 7], (float)nodes[i + 8])
                    }
                };
                triangles.Add(tri);
            }

            return triangles;
        }

        private static void BuildTopologyWithEps(
            List<Triangle> tris, float epsilon,
            out int[][] triVerts,
            out Dictionary<EdgeKey, List<int>> edgeToTris)
        {
            var n = tris.Count;
            triVerts = new int[n][];
            var inv = 1.0f / epsilon;

            // Map snapped vertices to sequential IDs
            var vtxIds = new Dictionary<VertexKeyEps, int>(Math.Max(4, n * 3));
            var nextId = 0;

            for (var i = 0; i < n; i++)
            {
                var t = tris[i];
                var ids = new int[3];
                for (var k = 0; k < 3; k++)
                {
                    var key = new VertexKeyEps(t.Vertices[k], inv);
                    if (!vtxIds.TryGetValue(key, out var id))
                    {
                        id = nextId++;
                        vtxIds.Add(key, id);
                    }

                    ids[k] = id;
                }

                triVerts[i] = ids;
            }

            edgeToTris = new Dictionary<EdgeKey, List<int>>(Math.Max(4, n * 3));
            for (var i = 0; i < n; i++)
            {
                var ids = triVerts[i];
                var e0 = new EdgeKey(ids[0], ids[1]);
                var e1 = new EdgeKey(ids[1], ids[2]);
                var e2 = new EdgeKey(ids[2], ids[0]);

                if (!edgeToTris.TryGetValue(e0, out var l0)) edgeToTris[e0] = l0 = new List<int>(2);
                l0.Add(i);
                if (!edgeToTris.TryGetValue(e1, out var l1)) edgeToTris[e1] = l1 = new List<int>(2);
                l1.Add(i);
                if (!edgeToTris.TryGetValue(e2, out var l2)) edgeToTris[e2] = l2 = new List<int>(2);
                l2.Add(i);
            }
        }

        /// <summary>
        ///     Computes the axis-aligned bounding box of a list of triangles.
        /// </summary>
        private static (Vector3 min, Vector3 max) ComputeBounds(List<Triangle> triangles)
        {
            if (triangles == null || triangles.Count == 0)
                return (Vector3.Zero, Vector3.Zero);

            var min = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
            var max = new Vector3(float.MinValue, float.MinValue, float.MinValue);

            foreach (var tri in triangles)
            foreach (var v in tri.Vertices)
            {
                min = Vector3.Min(min, v);
                max = Vector3.Max(max, v);
            }

            return (min, max);
        }

        /// <summary>
        ///     Calculates an adaptive epsilon based on the model's bounding box diagonal.
        ///     Returns 0.001% of the diagonal length as a tolerance value.
        /// </summary>
        private static float CalculateAdaptiveEpsilon(List<Triangle> triangles, float fallback = 0.000001f)
        {
            if (triangles == null || triangles.Count == 0) return fallback;

            var (min, max) = ComputeBounds(triangles);
            var diagonal = Vector3.Distance(min, max);

            if (diagonal < 1e-10f) return fallback; // Model too small, use fallback

            // Use 0.001% of diagonal as epsilon
            return diagonal * 0.00001f;
        }

        private static void WriteAsciiStl(string path, List<Triangle> triangles, string solidName = "body")
        {
            using var writer = new StreamWriter(path);
            writer.WriteLine($"solid {solidName}");

            foreach (var tri in triangles)
            {
                writer.WriteLine("  facet normal 0 0 0");
                writer.WriteLine("    outer loop");
                foreach (var v in tri.Vertices) writer.WriteLine($"      vertex {v.X} {v.Y} {v.Z}");
                writer.WriteLine("    endloop");
                writer.WriteLine("  endfacet");
            }

            writer.WriteLine($"endsolid {solidName}");
        }


        public static List<List<Triangle>> GetConnectedComponents(List<Triangle> triangles, float? epsilon = null)
        {
            var n = triangles.Count;
            var result = new List<List<Triangle>>();
            if (n == 0) return result;

            var eps = epsilon ?? CalculateAdaptiveEpsilon(triangles);
            BuildTopologyWithEps(triangles, eps, out var triVerts, out var edgeToTris);

            var visited = new bool[n];
            var queue = new Queue<int>(Math.Min(1024, n));

            for (var i = 0; i < n; i++)
            {
                if (visited[i]) continue;
                var compIdxs = new List<int>();
                visited[i] = true;
                queue.Enqueue(i);

                while (queue.Count > 0)
                {
                    var current = queue.Dequeue();
                    compIdxs.Add(current);
                    var ids = triVerts[current];

                    if (edgeToTris.TryGetValue(new EdgeKey(ids[0], ids[1]), out var neigh))
                        foreach (var nb in neigh.Where(nb => !visited[nb]))
                        {
                            visited[nb] = true;
                            queue.Enqueue(nb);
                        }

                    if (edgeToTris.TryGetValue(new EdgeKey(ids[1], ids[2]), out neigh))
                        foreach (var nb in neigh.Where(nb => !visited[nb]))
                        {
                            visited[nb] = true;
                            queue.Enqueue(nb);
                        }

                    if (edgeToTris.TryGetValue(new EdgeKey(ids[2], ids[0]), out neigh))
                        foreach (var nb in neigh.Where(nb => !visited[nb]))
                        {
                            visited[nb] = true;
                            queue.Enqueue(nb);
                        }
                }

                var group = new List<Triangle>(compIdxs.Count);
                for (var k = 0; k < compIdxs.Count; k++) group.Add(triangles[compIdxs[k]]);
                result.Add(group);
            }

            return result;
        }

        /// <summary>
        ///     Filters connected components to remove bodies smaller than the specified minimum triangle count.
        ///     Components are sorted by size (largest first) before filtering.
        /// </summary>
        /// <param name="components">List of connected components to filter</param>
        /// <param name="minTriangleCount">Minimum number of triangles required for a body to be included</param>
        /// <returns>Filtered and sorted list of components</returns>
        public static List<List<Triangle>> FilterSmallBodies(List<List<Triangle>> components, int minTriangleCount = 1)
        {
            if (components == null || components.Count == 0) return new List<List<Triangle>>();

            // Sort by triangle count descending (largest first)
            var sorted = components.OrderByDescending(c => c.Count).ToList();

            // Filter out small bodies
            if (minTriangleCount > 1) sorted = sorted.Where(c => c.Count >= minTriangleCount).ToList();

            return sorted;
        }

        /// <summary>
        ///     Computes statistics for a body (list of triangles)
        /// </summary>
        public static BodyStatistics ComputeBodyStatistics(List<Triangle> triangles)
        {
            if (triangles == null || triangles.Count == 0)
                return new BodyStatistics();

            var (min, max) = ComputeBounds(triangles);

            float surfaceArea = 0;
            foreach (var tri in triangles)
            {
                // Calculate triangle area using cross product
                var v0 = tri.Vertices[0];
                var v1 = tri.Vertices[1];
                var v2 = tri.Vertices[2];

                var edge1 = v1 - v0;
                var edge2 = v2 - v0;
                var cross = Vector3.Cross(edge1, edge2);
                surfaceArea += cross.Length() * 0.5f;
            }

            return new BodyStatistics
            {
                TriangleCount = triangles.Count,
                SurfaceArea = surfaceArea,
                BoundingBoxMin = min,
                BoundingBoxMax = max
            };
        }

        /// <summary>
        ///     Validates the mesh topology and checks for degenerate triangles and manifold edges
        /// </summary>
        public static ValidationResults ValidateMesh(List<Triangle> triangles, float epsilon = 0.000001f)
        {
            var results = new ValidationResults
            {
                TotalTriangles = triangles?.Count ?? 0
            };

            if (triangles == null || triangles.Count == 0) return results;

            // Check for degenerate triangles
            foreach (var tri in triangles)
            {
                var v0 = tri.Vertices[0];
                var v1 = tri.Vertices[1];
                var v2 = tri.Vertices[2];

                var edge1 = v1 - v0;
                var edge2 = v2 - v0;
                var cross = Vector3.Cross(edge1, edge2);
                var area = cross.Length() * 0.5f;

                if (area < epsilon) results.DegenerateTriangles++;
            }

            // Build topology and check edge manifoldness
            var adaptiveEps = CalculateAdaptiveEpsilon(triangles, epsilon);
            BuildTopologyWithEps(triangles, adaptiveEps, out _, out var edgeToTris);

            foreach (var kvp in edgeToTris)
            {
                var count = kvp.Value.Count;
                if (count == 1)
                    results.BoundaryEdges++;
                else if (count == 2)
                    results.ManifoldEdges++;
                else
                    results.NonManifoldEdges++;
            }

            return results;
        }

        /// <summary>
        ///     Separates multiple bodies from an STL file and exports them to individual files.
        ///     Bodies are sorted by size (largest first) and optionally filtered by minimum triangle count.
        /// </summary>
        /// <param name="inFile">Input STL file path</param>
        /// <param name="outDir">Output directory for separated body files</param>
        /// <param name="minTriangleCount">Minimum triangle count to include a body (default: 1)</param>
        /// <param name="epsilon">Optional tolerance for vertex snapping (default: auto-calculated)</param>
        public static void SeparateBodies(string inFile, string outDir, int minTriangleCount = 1)
        {
            var triangles = ParseStl(inFile);
            var bodies = GetConnectedComponents(triangles);

            // Filter and sort bodies by size
            bodies = FilterSmallBodies(bodies, minTriangleCount);

            var baseName = Path.GetFileNameWithoutExtension(inFile);

            for (var i = 0; i < bodies.Count; i++)
            {
                var stats = ComputeBodyStatistics(bodies[i]);

                // Generate filename with body index and triangle count
                var fileName = $"{baseName}_body_{i + 1:D2}_tris{stats.TriangleCount}.stl";
                var outputPath = Path.Combine(outDir, fileName);
                var solidName = $"{baseName}_body_{i + 1}";

                WriteAsciiStl(outputPath, bodies[i], solidName);
            }
        }

        #region Nested type: BodyStatistics

        /// <summary>
        ///     Statistics about a body (connected component)
        /// </summary>
        public class BodyStatistics
        {
            public int TriangleCount { get; set; }
            public float SurfaceArea { get; set; }
            public Vector3 BoundingBoxMin { get; set; }
            public Vector3 BoundingBoxMax { get; set; }
            public Vector3 BoundingBoxSize => BoundingBoxMax - BoundingBoxMin;
            public float BoundingBoxVolume => BoundingBoxSize.X * BoundingBoxSize.Y * BoundingBoxSize.Z;
        }

        #endregion

        #region Nested type: EdgeKey

        // --- Optimized topology helpers (vertex snapping variant) ---
        private readonly struct EdgeKey : IEquatable<EdgeKey>
        {
            private readonly int _a; // canonicalized so A <= B
            private readonly int _b; // canonicalized so A <= B

            public EdgeKey(int u, int v)
            {
                if (u <= v)
                {
                    _a = u;
                    _b = v;
                }
                else
                {
                    _a = v;
                    _b = u;
                }
            }

            public bool Equals(EdgeKey other)
            {
                return _a == other._a && _b == other._b;
            }

            public override bool Equals(object obj)
            {
                return obj is EdgeKey o && Equals(o);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    return (_a * 397) ^ _b;
                }
            }
        }

        #endregion

        #region Nested type: ValidationResults

        /// <summary>
        ///     Validation results for mesh topology
        /// </summary>
        public class ValidationResults
        {
            public int TotalTriangles { get; set; }
            public int DegenerateTriangles { get; set; }
            public int ManifoldEdges { get; set; }
            public int BoundaryEdges { get; set; }
            public int NonManifoldEdges { get; set; }
            public bool IsManifold => NonManifoldEdges == 0;
        }

        #endregion

        #region Nested type: VertexKeyEps

        // Quantized vertex key for tolerant matching
        private readonly struct VertexKeyEps : IEquatable<VertexKeyEps>
        {
            private readonly int _x; // quantized coordinates
            private readonly int _y; // quantized coordinates
            private readonly int _z; // quantized coordinates

            public VertexKeyEps(Vector3 v, float invEps)
            {
                _x = (int)Math.Round(v.X * invEps);
                _y = (int)Math.Round(v.Y * invEps);
                _z = (int)Math.Round(v.Z * invEps);
            }

            public bool Equals(VertexKeyEps other)
            {
                return _x == other._x && _y == other._y && _z == other._z;
            }

            public override bool Equals(object obj)
            {
                return obj is VertexKeyEps o && Equals(o);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    var h = 17;
                    h = h * 31 + _x;
                    h = h * 31 + _y;
                    h = h * 31 + _z;
                    return h;
                }
            }
        }

        #endregion
    }
}