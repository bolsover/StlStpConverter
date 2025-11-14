using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Bolsover.Converter
{
    public class EdgeLoop : IEntity
    {
        public int Id { get; }
        public string Label { get; } = string.Empty;

        private List<OrientedEdge> OrientedEdges { get; }


        public EdgeLoop(int id, List<OrientedEdge> edgesIn)
        {
            Id = id;
            OrientedEdges = edgesIn;
        }

        // Serialize method
        public void Serialize(StreamWriter writer)
        {
            writer.Write($"#{Id} = EDGE_LOOP('{Label}', (");
            writer.Write(SerializeEdgeReferences());
            writer.WriteLine("));");
        }

        private string SerializeEdgeReferences()
        {
            return string.Join(",", OrientedEdges.Select(edge => $"#{edge.Id}"));
        }
    }
}