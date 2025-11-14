using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Bolsover.Converter;

namespace Bolsover.Converter
{
    public class ManifoldShape : IEntity
    {
        public int Id { get; }
        public string Label { get; } = string.Empty;

        private AxisPlacement3D AxisPlacement { get; }
        private ShellModel ShellModel { get; }


        public ManifoldShape(int id, AxisPlacement3D axisPlacementIn, ShellModel shellModelIn)
        {
            Id = id;
            AxisPlacement = axisPlacementIn;
            ShellModel = shellModelIn;
        }

        public void Serialize(StreamWriter writer)
        {
            writer.WriteLine(
                $"#{Id} = MANIFOLD_SURFACE_SHAPE_REPRESENTATION('{Label}', (#{AxisPlacement.Id}, #{ShellModel.Id}));");
        }
    }
}