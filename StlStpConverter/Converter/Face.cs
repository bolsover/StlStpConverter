using System.Collections.Generic;
using System.IO;

namespace Bolsover.Converter
{
    public class Face : IEntity
    {
        public Face(int id, List<FaceBound> faceBoundsIn, Plane planeIn, bool dirIn)
        {
            Id = id;
            FaceBounds = faceBoundsIn;
            Dir = dirIn;
            Plane = planeIn;
        }

        private List<FaceBound> FaceBounds { get; }
        private bool Dir { get; }
        private Plane Plane { get; }

        #region IEntity Members

        public int Id { get; }
        public string Label { get; } = string.Empty;

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

        #endregion
    }
}