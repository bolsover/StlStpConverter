using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Bolsover.Converter
{
    public class Face : IEntity
    {
        public int Id { get; }
        public string Label { get; } = string.Empty;

        private List<FaceBound> FaceBounds { get; }
        private bool Dir { get; }
        private Plane Plane { get; }


        public Face(int id, List<FaceBound> faceBoundsIn, Plane planeIn, bool dirIn)
        {
            Id = id;
            FaceBounds = faceBoundsIn;
            Dir = dirIn;
            Plane = planeIn;
        }

        public void Serialize(StreamWriter writer)
        {
            writer.Write($"#{Id} = ADVANCED_FACE('{Label}', (");
            for (var i = 0; i < FaceBounds.Count; i++)
            {
                writer.Write($"#{FaceBounds[i].Id}");
                if (i != FaceBounds.Count - 1)
                    writer.Write(",");
            }

            writer.WriteLine($"),#{Plane.Id},{(Dir ? ".T." : ".F.")});");
        }
    }
}