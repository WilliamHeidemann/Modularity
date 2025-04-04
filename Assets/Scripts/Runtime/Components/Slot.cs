using System;
using Runtime.Components.Segments;
using Runtime.Components.Utility;
using Runtime.Scriptable_Objects;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

namespace Runtime.Components
{
    public class Slot : MonoBehaviour
    {
        [SerializeField] private Builder _builder;
        [SerializeField] private PlaceHolderBuilder _placeHolderBuilder;
        [SerializeField] private Selection _selection;
        
        public Vector3Int Position;
        private bool _mousePressed;
        private bool _mouseIn;
        
        private void OnMouseEnter()
        {
            _placeHolderBuilder.Build(Position);
            _mouseIn = true;
        }

        private void OnMouseExit()
        {
            _placeHolderBuilder.Hide();
            _mousePressed = false;
            _mouseIn = false;
        }

        private void OnMouseUp()
        {
            if (!_mousePressed || EventSystem.current.IsPointerOverGameObject()) return;
            if (!_selection.Prefab.IsSome(out _)) return;
            _builder.Build(Position, _placeHolderBuilder.PlaceholderRotation());
            CameraControls.Instance.StartCameraShake();
            if (_builder.IsValidBuildAttempt(Position, _placeHolderBuilder.PlaceholderRotation(), out var _, out var _))
            {
                _placeHolderBuilder.TearDown();
            }
            else
            {
                SoundFXPlayer.Instance.Play(SoundFX.InvalidPlacement);
            }
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

        private void RefreshBuildableHover()
        {
            _placeHolderBuilder.Hide();
            _placeHolderBuilder.Build(Position);
        }

        private void Update()
        {
            if (!_mouseIn) return;
            if (Input.GetKeyUp(KeyCode.Alpha1) || Input.GetKeyUp(KeyCode.Alpha2) || Input.GetKeyUp(KeyCode.Alpha3))
            {
                RefreshBuildableHover();
            }
        }

    }
}