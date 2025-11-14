using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Bolsover.Converter
{
    public class Shell : IEntity
    {
        public int Id { get; }
        public string Label { get; } = string.Empty;

        private List<Face> Faces { get; }
        private bool IsOpen { get; }


        public Shell(int id, List<Face> faces)
        {
            Id = id;
            Faces = faces;
            IsOpen = true;
        }

        public void Serialize(StreamWriter writer)
        {
            writer.Write($"#{Id} = {(IsOpen ? "OPEN_SHELL" : "CLOSED_SHELL")}('{Label}',(");
            WriteFaceReferences(writer);
            writer.WriteLine("));");
        }


        private void WriteFaceReferences(StreamWriter writer)
        {
            writer.Write(string.Join(",", Faces.Select(f => $"#{f.Id}")));
        }
    }
}