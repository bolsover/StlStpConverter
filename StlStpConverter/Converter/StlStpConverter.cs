using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Bolsover.Export;
using Bolsover.Import;

namespace Bolsover.Converter
{
    public abstract class StlStpConverter
    {
        public static async Task<int> ConvertToStp(string inputFile, string outputFile, double tol = 1e-6)
        {
            return await ConvertToStp(inputFile, outputFile, tol, CancellationToken.None, null);
        }

        private static async Task<int> ConvertToStp(string inputFile, string outputFile, double tol,
            CancellationToken token,
            IProgress<string> progress)
        {
            progress?.Report($"Reading STL: {Path.GetFileName(inputFile)}...");
            // Read STL file (async)
            var nodes = await StlReader.ReadStlAsync(inputFile, token, progress);
            token.ThrowIfCancellationRequested();
            if (nodes.Count / 9 == 0)
            {
                var msg = $"No triangles found in STL file: {inputFile}";
                progress?.Report(msg);
                // Console.WriteLine(@msg);
                return 1;
            }

            var triCount = nodes.Count / 9;
            progress?.Report($"Read {triCount} triangles. Building STEP body...");
            //Console.WriteLine($@"Read {triCount} triangles from {inputFile}");

            // Build STEP body and write output
            var stepWriter = new StepWriter();
            var mergedEdgeCount = 0;
            token.ThrowIfCancellationRequested();
            stepWriter.BuildTriangularBody(nodes, tol, ref mergedEdgeCount);
            token.ThrowIfCancellationRequested();
            progress?.Report($"Writing STEP: {Path.GetFileName(outputFile)}...");
            stepWriter.WriteStep(outputFile);

            var mergedMsg = $"Merged {mergedEdgeCount} edges";
            progress?.Report(mergedMsg);
            progress?.Report($@"Exported STEP file: {outputFile}");
            progress?.Report($"Done. {mergedMsg}. Saved: {outputFile}");

            return 0;
        }
    }
}