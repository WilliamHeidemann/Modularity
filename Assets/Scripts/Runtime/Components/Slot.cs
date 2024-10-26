using System;
using Runtime.Components.Segments;
using Runtime.Scriptable_Objects;
using UnityEngine;
using UnityEngine.Serialization;

namespace Runtime.Components
{
    public class Slot : MonoBehaviour
    {
        [SerializeField] private Builder _builder;
        [SerializeField] private PlaceHolderBuilder _placeHolderBuilder;
        public Vector3Int Position;
        
        private void OnMouseEnter()
        {
            _placeHolderBuilder.Build(Position);
        }

        private void OnMouseExit()
        {
            _placeHolderBuilder.TearDown();
        }

        private void OnMouseDown()
        {
            _placeHolderBuilder.TearDown();
            _builder.Build(Position);
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireCube(transform.position, Vector3.one);
        }
    }
}