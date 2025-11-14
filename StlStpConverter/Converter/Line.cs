using System;
using System.Collections.Generic;
using System.IO;

namespace Bolsover.Converter
{
    public class Line : IEntity
    {
        public int Id { get; }
        public string Label { get; } = string.Empty;

        private CartesianPoint CartesianPoint { get; }
        private Vector Vector { get; }


        public Line(int id, CartesianPoint cartesianPointIn, Vector vectorIn)
        {
            Id = id;
            CartesianPoint = cartesianPointIn;
            Vector = vectorIn;
        }

        public void Serialize(StreamWriter writer)
        {
            writer.WriteLine($"#{Id} = LINE('{Label}', #{CartesianPoint.Id}, #{Vector.Id});");
        }
    }
}