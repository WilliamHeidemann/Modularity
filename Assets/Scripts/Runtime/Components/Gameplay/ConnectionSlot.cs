using UnityEngine;

namespace Runtime.Components.Gameplay
{
    public class ConnectionSlot : MonoSegment
    {
        [SerializeField] private GameObject _placeHolder;

        private void OnMouseEnter()
        {
            _placeHolder.SetActive(true);
        }

        private void OnMouseExit()
        {
            _placeHolder.SetActive(false);
        }

        private void OnMouseDown()
        {
            SegmentFactory.Instance.TryBuild(Segment.Position);
        }
    }
}