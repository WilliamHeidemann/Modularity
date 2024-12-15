using System.Linq;
using Runtime.Backend;
using Runtime.Scriptable_Objects;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

namespace Runtime.Components.Utility
{
    public class CameraControls : MonoBehaviour
    {
        [SerializeField] private float _keyboardSpeed;
        [SerializeField] private float _dragSpeed;
        [SerializeField] private float _rotationSpeed;
        [SerializeField] private float _zoomSpeed;
        [SerializeField] private float _rotationPointOffset;
        [SerializeField] private Structure _structure;
        [SerializeField] private float _resetConstant;
        [SerializeField] private float _resetPercentage;

        private Vector3 _dragOrigin;
        private Vector3 _mouseDelta;
        private Vector3 _mouseRightDelta;
        private Vector3 _startPosition;
        private Vector3 _startRotation;

        private bool _isGamePaused = true;

        private bool _wasClickedOverCard;

        private void OnEnable()
        {
            MainMenuController.OnGameStart += TogglePause;
            PauseMenuController.OnGamePause += TogglePause;
        }

        private void OnDisable()
        {
            MainMenuController.OnGameStart -= TogglePause;
            PauseMenuController.OnGamePause -= TogglePause;
        }

        private void Start()
        {
            _startPosition = transform.position;
            _startRotation = transform.eulerAngles;
        }

        public void TogglePause()
        {
            _isGamePaused = !_isGamePaused;
        }

        private void Update()
        {
            if (_isGamePaused)
            {
                return;
            }

            _mouseDelta = _dragOrigin - Input.mousePosition;
            _dragOrigin = Input.mousePosition;
            HandleRotationKeyboard();
            HandleRotationMouse();
            HandleDragTranslation();
            HandleZoom();
            var position = transform.position;
            position.x = Mathf.Clamp(transform.position.x, -25.0f, 25.0f);
            position.y = Mathf.Clamp(transform.position.y, 0.0f, 25.0f);
            position.z = Mathf.Clamp(transform.position.z, -25.0f, 25.0f);

            transform.position = position;
        }

        private void HandleDragTranslation()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (EventSystem.current.IsPointerOverGameObject())
                {
                    _wasClickedOverCard = true;
                    return;
                }

                _wasClickedOverCard = false;
                _dragOrigin = Input.mousePosition;
                return;
            }

            if (_wasClickedOverCard) return;

            if (!Input.GetMouseButton(0))
            {
                return;
            }

            var translation = new Vector3(_mouseDelta.x * _dragSpeed, _mouseDelta.y * _dragSpeed, 0);
            transform.Translate(translation, Space.Self);
            PreventGoingThroughFloor();
        }

        private void PreventGoingThroughFloor()
        {
            var position = transform.position;
            transform.position = new Vector3(position.x, Mathf.Abs(position.y), position.z);
        }

        private void HandleRotationKeyboard()
        {
            var xAxis = Input.GetAxis("Horizontal");
            var yAxis = Input.GetAxis("Vertical");

            if ((xAxis == 0 && yAxis == 0) || Input.GetMouseButton(0))
            {
                return;
            }

            Rotate(xAxis, yAxis);
        }

        private void HandleRotationMouse()
        {
            if (!Input.GetMouseButton(1))
            {
                _mouseRightDelta = Vector3.zero;
                return;
            }

            var xIncrement = GetIncrement(_mouseDelta.x);
            var yIncrement = GetIncrement(-_mouseDelta.y);

            if (!HasSameSign(xIncrement, _mouseRightDelta.x))
            {
                _mouseRightDelta = new Vector3(0, _mouseRightDelta.y, 0);
            }
            
            if (!HasSameSign(yIncrement, _mouseRightDelta.y))
            {
                _mouseRightDelta = new Vector3(_mouseRightDelta.x, 0, 0);
            }

            _mouseRightDelta += new Vector3(xIncrement, yIncrement, 0);

            const float limit = 2f;

            var xAxis = Mathf.Clamp(_mouseRightDelta.x, -limit, limit);
            var yAxis = Mathf.Clamp(_mouseRightDelta.y, -limit, limit);

            if ((xAxis == 0 && yAxis == 0) || Input.GetMouseButton(0))
            {
                return;
            }

            print(xAxis);
            print(yAxis);

            Rotate(xAxis, yAxis);
        }

        private static float GetIncrement(float delta)
        {
            const float threshold = 1.0f;
            const float increment = 0.02f;
            return delta switch
            {
                > threshold => increment,
                < -threshold => -increment,
                _ => 0,
            };
        }

        private static bool HasSameSign(float a, float b)
        {
            return (int)Mathf.Sign(a) == (int)Mathf.Sign(b);
        }

        private void Rotate(float xAxis, float yAxis)
        {
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
            var positions = _structure.Segments.Select(segment => segment.Position).ToList();
            var center = SpawnUtility.GetCenter(positions);
            var distance = SpawnUtility.GetRadius(positions, _resetConstant, _resetPercentage);
            var direction = transform.position - center;
            transform.position = center + direction.normalized * distance;
            transform.LookAt(center);
            _dragOrigin = center;
        }
    }
}