using Runtime.Components.Segments;
using UnityEngine;

namespace Runtime.Scriptable_Objects
{
    [CreateAssetMenu]
    public class PlaceHolderBuilder : ScriptableObject
    {
        [SerializeField] private Selection _selection;
        private Segment _placeHolder;

        public void Build(Vector3Int position)
        {
            TearDown();
            _placeHolder = Instantiate(_selection.Prefab, position, Quaternion.identity);
            _placeHolder.ConnectionPoints.Randomize();
        }

        public void TearDown()
        {
            if (_placeHolder != null)
            {
                Destroy(_placeHolder);
            }
        }
    }
}