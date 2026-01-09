using System.IO;
using System.Numerics;
using System.Text;

namespace Bolsover.Decimator
{
    public static class StlBinaryExporter
    {
        public static void SaveBinary(string path, StlSimplifier simplifier)
        {
            using (var stream = new FileStream(path, FileMode.Create, FileAccess.Write))
            using (var writer = new BinaryWriter(stream))
            {
                // 1. Write 80-byte header
                var header = "STL Exported by C# Simplifier";
                var headerBytes = new byte[80];
                Encoding.ASCII.GetBytes(header, 0, header.Length, headerBytes, 0);
                writer.Write(headerBytes);

                // 2. Write number of triangles
                var triangleCount = (uint)simplifier.Faces.Count;
                writer.Write(triangleCount);

                // 3. Write each triangle
                foreach (var face in simplifier.Faces)
                {
                    var v0 = simplifier.Vertices[face.Vertices[0]].Position;
                    var v1 = simplifier.Vertices[face.Vertices[1]].Position;
                    var v2 = simplifier.Vertices[face.Vertices[2]].Position;

                    // Compute normal
                    var normal = Vector3.Cross(v1 - v0, v2 - v0);
                    if (normal.LengthSquared() > 1e-12f)
                        normal = Vector3.Normalize(normal);
                    else
                        normal = Vector3.Zero;

                    // Write normal (3 floats)
                    writer.Write(normal.X);
                    writer.Write(normal.Y);
                    writer.Write(normal.Z);

                    // Write vertices (9 floats)
                    writer.Write(v0.X);
                    writer.Write(v0.Y);
                    writer.Write(v0.Z);
                    writer.Write(v1.X);
                    writer.Write(v1.Y);
                    writer.Write(v1.Z);
                    writer.Write(v2.X);
                    writer.Write(v2.Y);
                    writer.Write(v2.Z);

                    // Write attribute byte count (2 bytes, zero)
                    writer.Write((ushort)0);
                }
            }
        }
    }
}