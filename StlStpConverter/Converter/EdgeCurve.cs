
using System.IO;

namespace Bolsover.Converter
{
    public class EdgeCurve : IEntity
    {
        public int Id { get; }
        public string Label { get; } = string.Empty;
        private Vertex Vert1 { get; }
        private Vertex Vert2 { get; }

        private Line Line { get; }
        private bool Dir { get; }

      

        public EdgeCurve(int id, Vertex vert1In, Vertex vert2In, Line lineIn, bool dirIn)
        {
            Id = id;
            Vert1 = vert1In;
            Vert2 = vert2In;
            Line = lineIn;
            Dir = dirIn;
        }

        public void Serialize(StreamWriter writer)
        {
            var id = GetId();
            var direction = Dir ? ".T." : ".F.";
            writer.WriteLine($"#{Id} = EDGE_CURVE('', #{Vert1.Id}, #{Vert2.Id}, #{id}, {direction});");
        }


        private int GetId()
        {
        
            return Line?.Id ?? 0;
        }
    }
}