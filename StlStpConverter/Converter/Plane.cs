using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Bolsover.Converter;

namespace Bolsover.Converter
{
    public class Plane : IEntity
    {
        public int Id { get; }
        public string Label { get; } = string.Empty;
        private AxisPlacement3D AxisPlacement { get; }

        // Constructor with required parameter
        public Plane(int id, AxisPlacement3D axisPlacementIn)
        {
            Id = id;
            AxisPlacement = axisPlacementIn ?? throw new InvalidOperationException("AxisPlacement is required");
        }

        // Serialize method
        public void Serialize(StreamWriter writer)
        {
            writer.WriteLine($"#{Id} = PLANE('{Label}',#{AxisPlacement.Id});");
        }
    }
}