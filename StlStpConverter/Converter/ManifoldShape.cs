using System.IO;

namespace Bolsover.Converter
{
    public class ManifoldShape : IEntity
    {
        public ManifoldShape(int id, AxisPlacement3D axisPlacementIn, ShellModel shellModelIn)
        {
            Id = id;
            AxisPlacement = axisPlacementIn;
            ShellModel = shellModelIn;
        }

        private AxisPlacement3D AxisPlacement { get; }
        private ShellModel ShellModel { get; }

        #region IEntity Members

        public int Id { get; }
        public string Label { get; } = string.Empty;

        public void Serialize(StreamWriter writer)
        {
            writer.WriteLine(
                $"#{Id} = MANIFOLD_SURFACE_SHAPE_REPRESENTATION('{Label}', (#{AxisPlacement.Id}, #{ShellModel.Id}));");
        }

        #endregion
    }
}