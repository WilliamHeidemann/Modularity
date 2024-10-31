using Runtime.Components.Segments;
using UnityEngine;
using UtilityToolkit.Runtime;

namespace Runtime.Scriptable_Objects
{
    [CreateAssetMenu]
    public class PlaceHolderBuilder : ScriptableObject
    {
        [SerializeField] private Selection _selection;
        [SerializeField] private Structure _structure;
        private Option<Segment> _placeHolder;

        public void Build(Vector3Int position)
        {
            if (!_structure.IsOpenPosition(position))
            {
                return;
            }

            TearDown();
            var placeHolder = Instantiate(_selection.Prefab, position, Quaternion.identity);
            placeHolder.GetComponent<BoxCollider>().enabled = false;
            _placeHolder = Option<Segment>.Some(placeHolder);
        }

        public void TearDown()
        {
            if (_placeHolder.IsSome(out var segment))
            {
                Destroy(segment.gameObject);
            }
            _placeHolder = Option<Segment>.None;
        }

        public Quaternion PlaceholderRotation()
        {
            Debug.Log(_placeHolder.IsSome(out var _));
            return _placeHolder.IsSome(out var segment) ? segment.transform.rotation : Quaternion.identity;
        }

        public void RotateOnY() => Rotate(Vector3.up);
        public void RotateOnX() => Rotate(Vector3.right);
        private void Rotate(Vector3 axis)
        {
            if (!_placeHolder.IsSome(out var segment))
            {
                return;
            }
            
            segment.transform.Rotate(axis, 90, Space.World);
        }
    }
}