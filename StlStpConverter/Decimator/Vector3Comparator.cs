using System.Collections.Generic;
using System.Numerics;

namespace Bolsover.Decimator
{
    public class Vector3Comparer : IEqualityComparer<Vector3>
    {
        #region IEqualityComparer<Vector3> Members

        public bool Equals(Vector3 a, Vector3 b)
        {
            return Vector3.Distance(a, b) < 1e-6;
        }

        public int GetHashCode(Vector3 v)
        {
            return v.GetHashCode();
        }

        #endregion
    }
}