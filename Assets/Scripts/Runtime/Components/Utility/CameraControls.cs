using UnityEngine;

namespace Runtime.Components.Utility
{
    public class CameraControls : MonoBehaviour
    {
        private const float RotationSpeed = 10f;
        private const float TranslationSpeed = 250f;
        private float _lastMouseX;
        private float _lastMouseY;

        private void Update()
        {
            HandleRotation();
            HandleTranslation();
        }

        private void HandleTranslation()
        {
            var xTranslation = Input.GetAxis("Horizontal") * Time.deltaTime;
            var zTranslation = Input.GetAxis("Vertical") * Time.deltaTime;
            var translation = new Vector3(xTranslation, 0, zTranslation) * (TranslationSpeed * Time.deltaTime);
            transform.Translate(translation, Space.Self);
        }

        private void HandleRotation()
        {
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                Cursor.visible = false;
                var deltaX = Input.mousePosition.x - _lastMouseX;
                var deltaY = Input.mousePosition.y - _lastMouseY;
                if (!Input.GetKeyDown(KeyCode.LeftShift) && !Input.GetKeyDown(KeyCode.RightShift))
                {
                    transform.Rotate(Vector3.up, deltaX * RotationSpeed * Time.deltaTime, Space.World);
                    transform.Rotate(Vector3.left, deltaY * RotationSpeed * Time.deltaTime, Space.Self);
                }
                _lastMouseX = Input.mousePosition.x;
                _lastMouseY = Input.mousePosition.y;
            }
            else
            {
                Cursor.visible = true;
            }
        }
    }
}