using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Bolsover.Converter
{
    public class Shell : IEntity
    {
        public Shell(int id, List<Face> faces)
        {
            Id = id;
            Faces = faces;
            IsOpen = true;
        }

        public Shell(int id, List<Face> faces, bool open)
        {
            Id = id;
            Faces = faces;
            IsOpen = open;
        }

        private List<Face> Faces { get; }
        private bool IsOpen { get; }

        #region IEntity Members

        public int Id { get; }
        public string Label { get; } = string.Empty;

        public void Serialize(StreamWriter writer)
        {
            writer.Write($"#{Id} = {(IsOpen ? "OPEN_SHELL" : "CLOSED_SHELL")}('{Label}',(");
            WriteFaceReferences(writer);
            writer.WriteLine("));");
        }

        #endregion


        private void WriteFaceReferences(StreamWriter writer)
        {
            writer.Write(string.Join(",", Faces.Select(f => $"#{f.Id}")));
        }
    }
}