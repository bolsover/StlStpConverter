using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bolsover.Converter
{
    public class AdvancedBrepShape : IEntity
    {
        public int Id { get; }
        public string Label { get; } = string.Empty;

        private GeometricRepresentation GeometricRepresentation { get; }
        private ShellModel ShellModel { get; }


        public AdvancedBrepShape(int id, ShellModel shellModelIn, GeometricRepresentation geometricRepresentation)
        {
            Id = id;
            ShellModel = shellModelIn;
            GeometricRepresentation = geometricRepresentation;
        }

        public void Serialize(StreamWriter writer)
        {
             writer.WriteLine($"#{Id} = ADVANCED_BREP_SHAPE_REPRESENTATION('{Label}', (#{ShellModel.Id}), #{GeometricRepresentation.Id});");
        }
    }
}