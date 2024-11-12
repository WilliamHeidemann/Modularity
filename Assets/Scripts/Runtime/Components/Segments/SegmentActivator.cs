using Runtime.Scriptable_Objects;
using UnityEngine;

namespace Runtime.Components.Segments
{
    public class SegmentActivator : MonoBehaviour
    {
        [SerializeField] public bool IsActive;
        public virtual void Activate(){
            if (!IsActive)
            {
                IsActive = true;
            }
        }
        public virtual void Activate(Vector3Int source, Vector3Int connection){}
    }
}