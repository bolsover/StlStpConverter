using System.Collections.Generic;
using System.IO;

namespace Bolsover.Converter
{
    public class Vertex : IEntity
    {
        public int Id { get; }
        public string Label { get; } = string.Empty;
        public CartesianPoint CartesianPoint { get; }


        public Vertex(int id, CartesianPoint cartesianPointIn)
        {
            Id = id;
            CartesianPoint = cartesianPointIn;
        }

        // Serialize method
        public void Serialize(StreamWriter writer)
        {
            writer.WriteLine($"#{Id} = VERTEX_POINT('{Label}', #{CartesianPoint.Id});");
        }
    }
}