using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Bolsover.Converter
{
    public class ShellModel : IEntity
    {
        public enum ShellModelTypes
        {
            Surface,
            Solid
        }

        public int Id { get; }
        public string Label { get; } = string.Empty;

        private List<Shell> Shells { get; }

        private ShellModelTypes ShellModelType { get; }

        public ShellModel(int id, List<Shell> shellsIn)
        {
            Id = id;
            Shells = shellsIn;
            ShellModelType = ShellModelTypes.Surface;
        }

        public ShellModel(int id, List<Shell> shellsIn, ShellModelTypes shellTypeIn)
        {
            Id = id;
            Shells = shellsIn;
            ShellModelType = shellTypeIn;
        }

        public void Serialize(StreamWriter writer)
        {
            var shellIds = SerializeShellIds();

            switch (ShellModelType)
            {
                case ShellModelTypes.Surface:
                    writer.WriteLine($"#{Id} = SHELL_BASED_SURFACE_MODEL('{Label}', ({shellIds}));");
                    break;

                case ShellModelTypes.Solid:
                    writer.WriteLine($"#{Id} = MANIFOLD_SOLID_BREP('{Label}', {shellIds});");
                    break;

                default:
                    throw new Exception("Unknown ShellModel type!");
            }
        }

        private string SerializeShellIds()
        {
            return string.Join(",", Shells.Select(shell => $"#{shell.Id}"));
        }
    }
}