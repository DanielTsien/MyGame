using UnityEngine;

namespace Utilities
{
    public static class UnityUtility
    {
        public static void Reset(this Transform transform, bool includeScale = true) {
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            if ( includeScale ) { transform.localScale = Vector3.one; }
        }
    }
}