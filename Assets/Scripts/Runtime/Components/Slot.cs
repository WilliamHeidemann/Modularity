using System;
using Runtime.Components.Segments;
using Runtime.Scriptable_Objects;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace Runtime.Components
{
    public class Slot : MonoBehaviour
    {
        [SerializeField] private Builder _builder;
        [SerializeField] private PlaceHolderBuilder _placeHolderBuilder;
        public Vector3Int Position;
        private bool _mousePressed;
        
        private void OnMouseEnter()
        {
            _placeHolderBuilder.Build(Position);
        }

        private void OnMouseExit()
        {
            _placeHolderBuilder.Hide();
            _mousePressed = false;
        }

        private void OnMouseUp()
        {
            if (!_mousePressed || EventSystem.current.IsPointerOverGameObject()) return;
            _builder.Build(Position, _placeHolderBuilder.PlaceholderRotation());
            _placeHolderBuilder.TearDown();
            _mousePressed = false;
        }
        private void OnMouseDown()
        {
            _mousePressed = true;
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(transform.position, 0.3f);
        }
    }
}