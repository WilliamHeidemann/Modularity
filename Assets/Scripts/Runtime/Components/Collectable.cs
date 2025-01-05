using Runtime.DataLayer;
using Runtime.Scriptable_Objects;
using UnityEngine;
using UnityEngine.Serialization;

namespace Runtime.Components
{
    public class Collectable : MonoBehaviour
    {
        public StaticSegmentData StaticSegmentData;
        public Vector3Int Position;
    }
}
