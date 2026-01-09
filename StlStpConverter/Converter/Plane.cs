using System;
using System.IO;

namespace Bolsover.Converter
{
    public class Plane : IEntity
    {
        // Constructor with required parameter
        public Plane(int id, AxisPlacement3D axisPlacementIn)
        {
            Id = id;
            AxisPlacement = axisPlacementIn ?? throw new InvalidOperationException("AxisPlacement is required");
        }

        private AxisPlacement3D AxisPlacement { get; }

        #region IEntity Members

        public int Id { get; }
        public string Label { get; } = string.Empty;

        // Serialize method
        public void Serialize(StreamWriter writer)
        {
            writer.WriteLine($"#{Id} = PLANE('{Label}',#{AxisPlacement.Id});");
        }

        #endregion
    }
}