using System.IO;

namespace Bolsover.Converter
{
    public interface IEntity
    {
        int Id { get; }
        string Label { get; }
        void Serialize(StreamWriter writer);
    }
}