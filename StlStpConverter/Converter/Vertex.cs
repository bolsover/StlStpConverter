using System.IO;

namespace Bolsover.Converter
{
    public class Vertex : IEntity
    {
        public Vertex(int id, CartesianPoint cartesianPointIn)
        {
            Id = id;
            CartesianPoint = cartesianPointIn;
        }

        public CartesianPoint CartesianPoint { get; }

        #region IEntity Members

        public int Id { get; }
        public string Label { get; } = string.Empty;

        // Serialize method
        public void Serialize(StreamWriter writer)
        {
            writer.WriteLine($"#{Id} = VERTEX_POINT('{Label}', #{CartesianPoint.Id});");
        }

        #endregion
    }
}