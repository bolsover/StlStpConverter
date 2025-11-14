using System;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Bolsover.Converter
{
    public class Direction : IEntity
    {
        public int Id { get; }
        public string Label { get; } = string.Empty;
        private double X { get; }
        private double Y { get; }
        private double Z { get; }

        public Direction(int id, double xIn, double yIn, double zIn)
        {
            Id = id;
            X = xIn;
            Y = yIn;
            Z = zIn;
        }

        // Serialize method
        public void Serialize(StreamWriter stream)
        {
            stream.WriteLine($"#{Id} = DIRECTION('{Label}', ({X},{Y},{Z}));");
        }
    }
}