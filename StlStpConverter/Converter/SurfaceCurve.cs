using System;
using System.Collections.Generic;
using System.IO;

namespace Bolsover.Converter
{
    public class SurfaceCurve : IEntity
    {
        public int Id { get; }
        public string Label { get; } = string.Empty;

        private Line Line { get; }


        public SurfaceCurve(int id, Line surfaceCurveIn)
        {
            Id = id;
            Line = surfaceCurveIn;
        }

        public void Serialize(StreamWriter writer)
        {
            writer.WriteLine($"#{Id} = SURFACE_CURVE('{Label}', #{Line.Id});");
        }
    }
}