using System.IO;

namespace Bolsover.Converter
{
    public class Line : IEntity
    {
        public Line(int id, CartesianPoint cartesianPointIn, Vector vectorIn)
        {
            Id = id;
            CartesianPoint = cartesianPointIn;
            Vector = vectorIn;
        }

        private CartesianPoint CartesianPoint { get; }
        private Vector Vector { get; }

        #region IEntity Members

        public int Id { get; }
        public string Label { get; } = string.Empty;

        public void Serialize(StreamWriter writer)
        {
            writer.WriteLine($"#{Id} = LINE('{Label}', #{CartesianPoint.Id}, #{Vector.Id});");
        }

        #endregion
    }
}