using UnityEngine;

namespace Runtime.Components.Utility
{
    public class CameraControls : MonoBehaviour
    {
        [SerializeField] private float _keyboardSpeed;
        [SerializeField] private float _dragSpeed;
        [SerializeField] private float _rotationSpeed;
        [SerializeField] private float _zoomSpeed;

        private Vector3 _dragOrigin;

        private float _horizontalRotation;
        private float _verticalRotation;

        private Vector3 _startPosition;
        private Vector3 _startRotation;
        
        public float verticalLimit = 80f; // Limit for vertical rotation (to prevent flipping)
        public float distanceFromTarget = 10f; // Distance from the target object

        private float currentYaw = 0f; // Horizontal rotation
        private float currentPitch = 0f; // Vertical rotation
        

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
            HandleZoom();
            NewRotationSystem();
        }

        private void NewRotationSystem()
        {

            // Get input for rotation
            float horizontalInput = -Input.GetAxis("Horizontal"); // Arrow keys or A/D
            float verticalInput = -Input.GetAxis("Vertical"); // Arrow keys or W/S

            // Adjust yaw (horizontal rotation) and pitch (vertical rotation)
            currentYaw += horizontalInput * _rotationSpeed * Time.deltaTime;
            currentPitch -= verticalInput * _rotationSpeed * Time.deltaTime;

            // Clamp pitch to prevent flipping upside down
            currentPitch = Mathf.Clamp(currentPitch, -verticalLimit, verticalLimit);

            // Calculate the new position of the camera
            Quaternion rotation = Quaternion.Euler(currentPitch, currentYaw, 0f);
            Vector3 offset = rotation * new Vector3(0, 0, -distanceFromTarget);

            // Set the camera's position and look at the target
            transform.position = Vector3.zero + offset;
            transform.LookAt(Vector3.zero);
        }

        private void HandleKeyboardTranslation()
        {
            var horizontal = Input.GetAxis("Horizontal");
            var vertical = Input.GetAxis("Vertical");
            var translation = new Vector3(horizontal, 0, vertical) * (_keyboardSpeed * Time.deltaTime);
            transform.Translate(translation, Space.Self);
        }

        private void HandleDragTranslation()
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
            transform.Translate(0, 0, scroll * _dragSpeed * _zoomSpeed * 1000);
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