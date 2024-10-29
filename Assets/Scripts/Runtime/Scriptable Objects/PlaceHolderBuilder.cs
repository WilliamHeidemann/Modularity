using Runtime.Components.Segments;
using UnityEngine;

namespace Runtime.Scriptable_Objects
{
    [CreateAssetMenu]
    public class PlaceHolderBuilder : ScriptableObject
    {
        [SerializeField] private Selection _selection;
        [SerializeField] private Structure _structure;
        private Segment _placeHolder;

        public void Build(Vector3Int position)
        {
            if (_structure.IsOpenPosition(position))
            {
                return;
            }

            TearDown();
            _placeHolder = Instantiate(_selection.Prefab, position, Quaternion.identity);
        }

        public void TearDown()
        {
            if (_placeHolder != null)
            {
                Destroy(_placeHolder.gameObject);
            }
        }

        public Quaternion PlaceholderRotation()
        {
            return _placeHolder == null 
                ? Quaternion.identity 
                : _placeHolder.transform.rotation;
        }

        public void RotateOnY() => Rotate(Vector3.up);
        public void RotateOnX() => Rotate(Vector3.right);
        private void Rotate(Vector3 axis)
        {
            if (_placeHolder == null)
            {
                return;
            }

            _placeHolder.transform.Rotate(axis, 90, Space.World);
        }
    }
}