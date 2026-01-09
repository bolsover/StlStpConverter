using System.IO;

namespace Bolsover.Converter
{
    public class GeometricRepresentation : IEntity
    {
        public GeometricRepresentation(int id, Unit uncertainty, Unit lengthUnits, Unit planeAngleUnits,
            Unit solidAngleUnits)
        {
            Id = id;
            Uncertainty = uncertainty;
            LengthUnits = lengthUnits;
            PlaneAngleUnits = planeAngleUnits;
            SolidAngleUnits = solidAngleUnits;
        }

        private Unit Uncertainty { get; }
        private Unit LengthUnits { get; }
        private Unit PlaneAngleUnits { get; }
        private Unit SolidAngleUnits { get; }

        #region IEntity Members

        public int Id { get; }

        public string Label { get; } = string.Empty;

        public void Serialize(StreamWriter writer)
        {
            writer.WriteLine($"#{Id} = (\n" +
                             "GEOMETRIC_REPRESENTATION_CONTEXT(3)\n" +
                             $"GLOBAL_UNCERTAINTY_ASSIGNED_CONTEXT((#{Uncertainty.Id}))\n" +
                             $"GLOBAL_UNIT_ASSIGNED_CONTEXT((#{SolidAngleUnits.Id},#{PlaneAngleUnits.Id},#{LengthUnits.Id}))\n" +
                             "REPRESENTATION_CONTEXT('ID1', '3D')\n" +
                             ");");
        }

        #endregion
    }
}