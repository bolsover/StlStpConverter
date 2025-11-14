using System;
using System.IO;
using System.Linq;
using System.Globalization;

namespace Bolsover.Converter
{
    public class CartesianPoint : IEntity
    {
        public int Id { get; }
        public string Label { get; } = string.Empty;
        public double X { get; }
        public double Y { get; }
        public double Z { get; }

        public CartesianPoint(int id, double xIn, double yIn, double zIn)
        {
            Id = id;
            X = xIn;
            Y = yIn;
            Z = zIn;
        }

        // Serialize method
        public void Serialize(StreamWriter writer)
        {
            writer.WriteLine($"#{Id} = CARTESIAN_POINT('{Label}', ({X},{Y},{Z}));");
        }
    }
}