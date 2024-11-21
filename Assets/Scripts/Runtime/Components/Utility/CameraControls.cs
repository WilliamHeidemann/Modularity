using UnityEngine;

namespace Runtime.Components.Utility
{
    public class CameraControls : MonoBehaviour
    {
        [SerializeField] private float _keyboardSpeed;
        [SerializeField] private float _dragSpeed;
        [SerializeField] private float _rotationSpeed;
        [SerializeField] private float _zoomSpeed;
        [SerializeField] private float _rotationPointOffset;

        private Vector3 _dragOrigin;
        private Vector3 _startPosition;
        private Vector3 _startRotation;

        private void Start()
        {
            _startPosition = transform.position;
            _startRotation = transform.eulerAngles;
        }

        private void Update()
        {
            HandleRotation();
            HandleDragTranslation();
            HandleZoom();
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
            PreventGoingThroughFloor();
            _dragOrigin = Input.mousePosition;
        }

        private void PreventGoingThroughFloor()
        {
            var position = transform.position;
            transform.position = new Vector3(position.x, Mathf.Abs(position.y), position.z);
        }

        private void HandleRotation()
        {
            var xAxis = Input.GetAxis("Horizontal");
            var yAxis = Input.GetAxis("Vertical");

            if ((xAxis == 0 && yAxis == 0) || Input.GetMouseButton(0))
            {
                return;
            }

            var xValue = transform.rotation.eulerAngles.x;

            var isGoingTooHigh = xValue is > 80 and < 100f && yAxis > 0f;
            var isGoingTooLow = transform.position.y < 0f && yAxis < 0f;

            if (isGoingTooHigh || isGoingTooLow)
            {
                yAxis = 0;
            }

            var xRotation = -xAxis * _rotationSpeed * Time.deltaTime;
            var yRotation = yAxis * _rotationSpeed * Time.deltaTime;
            var rotationPoint = transform.position + transform.forward * _rotationPointOffset;

            transform.RotateAround(rotationPoint, Vector3.up, xRotation);
            transform.RotateAround(rotationPoint, transform.right, yRotation);
            transform.LookAt(rotationPoint);
        }

        private void HandleZoom()
        {
            var zoom = Input.GetAxis("Mouse ScrollWheel") * _zoomSpeed * Time.deltaTime;
            if (zoom == 0f)
            {
                return;
            }
            
            transform.Translate(0, 0, zoom);
            PreventGoingThroughFloor();
        }

        public void ResetCamera()
        {
            transform.position = _startPosition;
            transform.rotation = Quaternion.Euler(_startRotation);
            _dragOrigin = new Vector3(0, 0, 0);
        }
    }
}