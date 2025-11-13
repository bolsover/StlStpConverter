using System.Numerics;
using System.Collections.Generic;

namespace Bolsover.Splitterator
{
    public class Triangle
    {
        public Vector3[] Vertices = new Vector3[3];

        public IEnumerable<(Vector3, Vector3)> GetEdges()
        {
            yield return (Vertices[0], Vertices[1]);
            yield return (Vertices[1], Vertices[2]);
            yield return (Vertices[2], Vertices[0]);
        }
    }
}