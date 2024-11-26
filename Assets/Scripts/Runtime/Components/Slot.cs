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
        private bool MousePressed;
        
        private void OnMouseEnter()
        {
            _placeHolderBuilder.Build(Position);
        }

        private void OnMouseExit()
        {
            _placeHolderBuilder.Hide();
            MousePressed = false;
        }

        private void OnMouseUp()
        {
            if (!MousePressed || EventSystem.current.IsPointerOverGameObject()) return;
            _builder.Build(Position, _placeHolderBuilder.PlaceholderRotation());
            _placeHolderBuilder.TearDown();
            MousePressed = false;
        }
        private void OnMouseDown()
        {
            MousePressed = true;
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(transform.position, 0.3f);
        }
    }
}