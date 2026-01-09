using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Bolsover.Converter
{
    public class EdgeLoop : IEntity
    {
        public EdgeLoop(int id, List<OrientedEdge> edgesIn)
        {
            Id = id;
            OrientedEdges = edgesIn;
        }

        private List<OrientedEdge> OrientedEdges { get; }

        #region IEntity Members

        public int Id { get; }
        public string Label { get; } = string.Empty;

        // Serialize method
        public void Serialize(StreamWriter writer)
        {
            writer.Write($"#{Id} = EDGE_LOOP('{Label}', (");
            writer.Write(SerializeEdgeReferences());
            writer.WriteLine("));");
        }

        #endregion

        private string SerializeEdgeReferences()
        {
            return string.Join(",", OrientedEdges.Select(edge => $"#{edge.Id}"));
        }
    }
}