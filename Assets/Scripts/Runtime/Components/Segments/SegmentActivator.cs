using Runtime.Scriptable_Objects;
using UnityEngine;

namespace Runtime.Components.Segments
{
    public class SegmentActivator : MonoBehaviour
    {
        [SerializeField] public bool IsActive;
        public void Activate(){
            if (!IsActive)
            {
                Debug.Log(this.gameObject.name);
                IsActive = true;
            }
        }
    }
}