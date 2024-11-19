using UnityEngine;
using UnityEngine.Serialization;

namespace Runtime.Components.Utility
{
    public class CameraControls : MonoBehaviour
    {
        [SerializeField] private float _rotationSpeed;
        [SerializeField] private float _zoomSpeed;
        [SerializeField] private float _minZoom;
        [SerializeField] private float _maxZoom;

        private Vector3 _dragOrigin;

        private float _horizontalRotation;
        private float _verticalRotation;

        private Vector3 _startPosition;
        private Vector3 _startRotation;

        private const float VerticalLimit = 80f;
        private float _distanceFromTarget = 10f;

        private float _currentHorizontalRotation;
        private float _currentVerticalRotation;


        private void Start()
        {
            _horizontalRotation = transform.eulerAngles.y;
            _verticalRotation = transform.eulerAngles.x;
            _startPosition = transform.position;
            _startRotation = transform.eulerAngles;
        }

        private void Update()
        {
            // HandleKeyboardTranslation();
            // HandleDragTranslation();
            // HandleRotation();
            // HandleZoom();
            NewRotationSystem();
        }

        private void NewRotationSystem()
        {
            var scroll = Input.GetAxis("Mouse ScrollWheel");
            _distanceFromTarget = Mathf.Clamp(_distanceFromTarget - scroll * _zoomSpeed, _minZoom, _maxZoom);

            var horizontalInput = Input.GetAxis("Horizontal"); // Arrow keys or A/D
            var verticalInput = Input.GetAxis("Vertical"); // Arrow keys or W/S

            _currentHorizontalRotation -= horizontalInput * _rotationSpeed * Time.deltaTime;
            _verticalRotation += verticalInput * _rotationSpeed * Time.deltaTime;

            _verticalRotation = Mathf.Clamp(_verticalRotation, -VerticalLimit, VerticalLimit);

            var rotation = Quaternion.Euler(_verticalRotation, _currentHorizontalRotation, 0f);
            var offset = rotation * new Vector3(0, 0, -_distanceFromTarget);

            transform.position = Vector3.zero + offset;
            transform.LookAt(Vector3.zero);
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