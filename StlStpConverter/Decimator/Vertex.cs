using System.Collections.Generic;
using System.Numerics;

namespace Bolsover.Decimator
{
    public class Vertex
    {
        public List<int> AdjacentFaces = new();
        public Vector3 Position;
        public Matrix4x4 Quadric = Matrix4x4.Identity;
    }
}