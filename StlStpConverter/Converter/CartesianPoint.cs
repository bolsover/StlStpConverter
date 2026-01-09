using System.IO;

namespace Bolsover.Converter
{
    public class CartesianPoint : IEntity
    {
        public CartesianPoint(int id, double xIn, double yIn, double zIn)
        {
            Id = id;
            X = xIn;
            Y = yIn;
            Z = zIn;
        }

        public double X { get; }
        public double Y { get; }
        public double Z { get; }

        #region IEntity Members

        public int Id { get; }
        public string Label { get; } = string.Empty;

        // Serialize method
        public void Serialize(StreamWriter writer)
        {
            writer.WriteLine(
                $"#{Id} = CARTESIAN_POINT('{Label}', ({X:0.000000000000000},{Y:0.000000000000000},{Z:0.000000000000000}));"); // was {X},{Y},{Z}
        }

        #endregion
    }
}