using System.Globalization;
using System.IO;

namespace Bolsover.Converter
{
    public class Vector : IEntity
    {
        public Vector(int id, Direction dirIn, double lenIn)
        {
            Id = id;
            Dir = dirIn;
            Length = lenIn;
        }

        private double Length { get; }
        private Direction Dir { get; }

        #region IEntity Members

        public int Id { get; }
        public string Label { get; } = string.Empty;

        // Serialize method
        public void Serialize(StreamWriter writer)
        {
            writer.WriteLine(
                $"#{Id} = VECTOR('{Label}', #{Dir.Id}, {Length.ToString("F10", CultureInfo.InvariantCulture)});");
        }

        #endregion
    }
}