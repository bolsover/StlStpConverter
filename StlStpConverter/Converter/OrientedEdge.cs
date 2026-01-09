using System.IO;

namespace Bolsover.Converter
{
    public class OrientedEdge : IEntity
    {
        public OrientedEdge(int id, EdgeCurve edgeCurveIn, bool dirIn)
        {
            Id = id;
            Edge = edgeCurveIn;
            IsForward = dirIn;
        }

        private bool IsForward { get; }
        private EdgeCurve Edge { get; }

        #region IEntity Members

        public int Id { get; }
        public string Label { get; } = string.Empty;

        // Serialize method
        public void Serialize(StreamWriter writer)
        {
            writer.WriteLine($"#{Id} = ORIENTED_EDGE('{Label}',*,*,#{Edge.Id},{(IsForward ? ".T." : ".F.")});");
        }

        #endregion
    }
}