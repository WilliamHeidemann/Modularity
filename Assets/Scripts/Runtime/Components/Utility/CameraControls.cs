using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Runtime.Components.Utility
{
    public class CameraControls : MonoBehaviour
    {
        [SerializeField] private float _dragSpeed;
        [SerializeField] private float _rotationSpeed;
        [SerializeField] private float _zoomSpeed;

        private Vector3 _dragOrigin;

        private float _horizontalRotation;
        private float _verticalRotation;

        private Vector3 _startPosition;
        private Vector3 _startRotation;

        private void Start()
        {
            _horizontalRotation = transform.eulerAngles.y;
            _verticalRotation = transform.eulerAngles.x;
            _startPosition = transform.position;
            _startRotation = transform.eulerAngles;
        }

        private void Update()
        {
            HandleTranslation();
            HandleRotation();
            HandleZoom();
        }

        private void HandleTranslation()
        {
            if (Input.GetMouseButtonDown(0))
            {
                _dragOrigin = Input.mousePosition;
                return;
            }

            if (!Input.GetMouseButton(0))
            {
                return;
            }

            var delta = Input.mousePosition - _dragOrigin;
            var translation = new Vector3(-delta.x * _dragSpeed, -delta.y * _dragSpeed, 0);
            transform.Translate(translation, Space.Self);
            _dragOrigin = Input.mousePosition;
        }

        private void HandleRotation()
        {
            if (!Input.GetMouseButton(1))
            {
                return;
            }
            
            var mouseX = Input.GetAxis("Mouse X");
            var mouseY = Input.GetAxis("Mouse Y");

            _horizontalRotation += mouseX * _rotationSpeed;
            _verticalRotation -= mouseY * _rotationSpeed;
            _verticalRotation = Mathf.Clamp(_verticalRotation, -90f, 90f);
            transform.rotation = Quaternion.Euler(_verticalRotation, _horizontalRotation, 0.0f);
        }
        
        private void HandleZoom()
        {
            var scroll = Input.GetAxis("Mouse ScrollWheel");
            transform.Translate(0, 0, scroll * _dragSpeed * _zoomSpeed);
        }

        public void ResetCamera()
        {
            transform.position = _startPosition;
            transform.rotation = Quaternion.Euler(_startRotation);
            _dragOrigin = new Vector3(0, 0, 0);
            _verticalRotation = _startRotation.x;
            _horizontalRotation = _startRotation.y;
        }
    }
}