using System.IO;

namespace Bolsover.Converter
{
    public class Direction : IEntity
    {
        public Direction(int id, double xIn, double yIn, double zIn)
        {
            Id = id;
            X = xIn;
            Y = yIn;
            Z = zIn;
        }

        private double X { get; }
        private double Y { get; }
        private double Z { get; }

        #region IEntity Members

        public int Id { get; }
        public string Label { get; } = string.Empty;

        // Serialize method
        public void Serialize(StreamWriter stream)
        {
            stream.WriteLine(
                $"#{Id} = DIRECTION('{Label}', ({X:0.000000000000000},{Y:0.000000000000000},{Z:0.000000000000000}));"); // was {X},{Y},{Z}
        }

        #endregion
    }
}