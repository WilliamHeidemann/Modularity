using System;
using Runtime.Scriptable_Objects;
using UnityEngine;
using UnityEngine.Serialization;

namespace Runtime.Components
{
    public class Slot : MonoBehaviour
    {
        [SerializeField] private Builder _builder;
        [SerializeField] private MeshFilter _placeHolder;
        [SerializeField] private SelectedSegment _selection;
        public Vector3Int Position;

        private void OnMouseEnter()
        {
            _placeHolder.mesh = _selection.Prefab.GetComponent<MeshFilter>().mesh;
            _placeHolder.gameObject.SetActive(true);
        }

        private void OnMouseExit()
        {
            _placeHolder.gameObject.SetActive(false);
        }

        private void OnMouseDown()
        {
            _builder.Build(Position);
        }
    }
}