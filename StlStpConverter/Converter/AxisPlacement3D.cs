using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;


namespace Bolsover.Converter
{
    public class AxisPlacement3D : IEntity
    {
        public int Id { get; }
        public string Label { get; } = string.Empty;
        private Direction Direction1 { get; }
        private Direction Direction2 { get; }
        private CartesianPoint CartesianPoint { get; }

        public AxisPlacement3D(int id, Direction direction1, Direction direction2, CartesianPoint cartesianPoint)
        {
            Id = id;
            Direction1 = direction1;
            Direction2 = direction2;
            CartesianPoint = cartesianPoint;
        }

        // Serialize method
        public void Serialize(StreamWriter writer)
        {
            writer.WriteLine($"#{Id} = AXIS2_PLACEMENT_3D('{Label}',#{CartesianPoint.Id},#{Direction1.Id},#{Direction2.Id});");
        }
    }
}