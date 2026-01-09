using System.IO;

namespace Bolsover.Converter
{
    public class Unit : IEntity
    {
        #region UnitTypes enum

        public enum UnitTypes
        {
            Length,
            PlaneAngle,
            SolidAngle,
            Uncertainty
        }

        #endregion

        public Unit(int id, UnitTypes unitType)
        {
            Id = id;
            UnitType = unitType;
        }

        public Unit(int id, UnitTypes unitType, Unit lengthUnits)
        {
            Id = id;
            UnitType = unitType;
            LengthUnits = lengthUnits;
        }

        public UnitTypes UnitType { get; }

        public Unit LengthUnits { get; }

        #region IEntity Members

        public int Id { get; }

        public string Label { get; } = string.Empty;

        public void Serialize(StreamWriter writer)
        {
            switch (UnitType)
            {
                case UnitTypes.Length:
                    writer.WriteLine($"#{Id} = (\n" +
                                     "LENGTH_UNIT()\n" +
                                     "NAMED_UNIT(*)\n" +
                                     "SI_UNIT(.MILLI.,.METRE.)\n" +
                                     ");");
                    break;

                case UnitTypes.PlaneAngle:
                    writer.WriteLine($"#{Id} = (\n" +
                                     "NAMED_UNIT(*)\n" +
                                     "PLANE_ANGLE_UNIT()\n" +
                                     "SI_UNIT($,.RADIAN.)\n" +
                                     ");");
                    break;

                case UnitTypes.SolidAngle:
                    writer.WriteLine($"#{Id} = (\n" +
                                     "NAMED_UNIT(*)\n" +
                                     "SI_UNIT($,.STERADIAN.)\n" +
                                     "SOLID_ANGLE_UNIT()\n" +
                                     ");");
                    break;

                case UnitTypes.Uncertainty:
                    writer.WriteLine(
                        $"#{Id} = UNCERTAINTY_MEASURE_WITH_UNIT(LENGTH_MEASURE(1.E-6),#{LengthUnits.Id},\n" +
                        "'DISTANCE_ACCURACY_VALUE',\n" +
                        "'Maximum model space distance between geometric entities at asserted connectivities'\n" +
                        ");");
                    break;
            }
        }

        #endregion
    }
}


/*
#99=(
LENGTH_UNIT()
NAMED_UNIT(*)
SI_UNIT(.MILLI.,.METRE.)
);

#100=(
NAMED_UNIT(*)
PLANE_ANGLE_UNIT()
SI_UNIT($,.RADIAN.)
);

#101=(
NAMED_UNIT(*)
SI_UNIT($,.STERADIAN.)
SOLID_ANGLE_UNIT()
);

#102=UNCERTAINTY_MEASURE_WITH_UNIT(LENGTH_MEASURE(1.E-6),#99,
'DISTANCE_ACCURACY_VALUE',
'Maximum model space distance between geometric entities at asserted c
onnectivities ');

#103=(
GEOMETRIC_REPRESENTATION_CONTEXT(3)
GLOBAL_UNCERTAINTY_ASSIGNED_CONTEXT((#102))
GLOBAL_UNIT_ASSIGNED_CONTEXT((#101,#100,#99))
REPRESENTATION_CONTEXT('ID1', '3D')
);


*/