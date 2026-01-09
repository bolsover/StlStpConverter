using System.IO;

namespace Bolsover.Converter
{
    public class AdvancedBrepShape : IEntity
    {
        public AdvancedBrepShape(int id, ShellModel shellModelIn, GeometricRepresentation geometricRepresentation)
        {
            Id = id;
            ShellModel = shellModelIn;
            GeometricRepresentation = geometricRepresentation;
        }

        private GeometricRepresentation GeometricRepresentation { get; }
        private ShellModel ShellModel { get; }

        #region IEntity Members

        public int Id { get; }
        public string Label { get; } = string.Empty;

        public void Serialize(StreamWriter writer)
        {
            writer.WriteLine(
                $"#{Id} = ADVANCED_BREP_SHAPE_REPRESENTATION('{Label}', (#{ShellModel.Id}), #{GeometricRepresentation.Id});");
        }

        #endregion
    }
}