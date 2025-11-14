using System;
using System.Collections.Generic;
using System.IO;

namespace Bolsover.Converter
{
    public class EdgeCurve : IEntity
    {
        public int Id { get; }
        public string Label { get; } = string.Empty;
        private Vertex Vert1 { get; }
        private Vertex Vert2 { get; }
        private SurfaceCurve SurfCurve { get; }
        private Line Line { get; }
        private bool Dir { get; }

        public EdgeCurve(int id, Vertex vert1In, Vertex vert2In, SurfaceCurve surfCurveIn, bool dirIn)
        {
            Id = id;
            Vert1 = vert1In;
            Vert2 = vert2In;
            SurfCurve = surfCurveIn;
            Dir = dirIn;
        }

        public void Serialize(StreamWriter writer)
        {
            var curveId = GetCurveId();
            var direction = Dir ? ".T." : ".F.";
            writer.WriteLine($"#{Id} = EDGE_CURVE('', #{Vert1.Id}, #{Vert2.Id}, #{curveId}, {direction});");
        }


        private int GetCurveId()
        {
            return SurfCurve?.Id ?? Line?.Id ?? 0;
        }
    }
}