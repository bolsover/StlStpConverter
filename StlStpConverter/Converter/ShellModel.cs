using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Bolsover.Converter
{
    public class ShellModel : IEntity
    {
        public int Id { get; }
        public string Label { get; } = string.Empty;

        private List<Shell> Shells { get; }


        public ShellModel(int id, List<Shell> shellsIn)
        {
            Id = id;
            Shells = shellsIn;
        }

        public void Serialize(StreamWriter writer)
        {
            var shellIds = SerializeShellIds();
            writer.WriteLine($"#{Id} = SHELL_BASED_SURFACE_MODEL('{Label}', ({shellIds}));");
        }

        private string SerializeShellIds()
        {
            return string.Join(",", Shells.Select(shell => $"#{shell.Id}"));
        }
    }
}