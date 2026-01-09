using System.IO;

namespace Bolsover.Converter
{
    public class FaceBound : IEntity
    {
        public FaceBound(int id, EdgeLoop edgeLoopIn, bool orientationIn)
        {
            Id = id;
            EdgeLoop = edgeLoopIn;
            Orientation = orientationIn;
        }

        private EdgeLoop EdgeLoop { get; }
        private bool Orientation { get; }

        #region IEntity Members

        public int Id { get; }
        public string Label { get; } = string.Empty;

        // Serialize method
        public void Serialize(StreamWriter writer)
        {
            writer.WriteLine($"#{Id} = FACE_BOUND('{Label}', #{EdgeLoop.Id},{(Orientation ? ".T." : ".F.")});");
        }

        #endregion
    }
}