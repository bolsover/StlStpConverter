using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Bolsover.Converter
{
    public class FaceBound : IEntity
    {
        public int Id { get; }
        public string Label { get; } = string.Empty;

        private EdgeLoop EdgeLoop { get; }
        private bool Orientation { get; }


        public FaceBound(int id, EdgeLoop edgeLoopIn, bool orientationIn)
        {
            Id = id;
            EdgeLoop = edgeLoopIn;
            Orientation = orientationIn;
        }

        // Serialize method
        public void Serialize(StreamWriter writer)
        {
            writer.WriteLine($"#{Id} = FACE_BOUND('{Label}', #{EdgeLoop.Id},{(Orientation ? ".T." : ".F.")});");
        }
    }
}