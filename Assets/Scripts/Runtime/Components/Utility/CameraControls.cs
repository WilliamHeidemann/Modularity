using UnityEngine;

namespace Runtime.Components.Utility
{
    public class CameraControls : MonoBehaviour
    {
        private const float RotationSpeed = 20f;
        private const float TranslationSpeed = 5f;
        private float _lastMouseX;

        private void Update()
        {
            HandleRotation();
            HandleTranslation();
        }

        private void HandleTranslation()
        {
            var xTranslation = Input.GetAxis("Horizontal") * Time.deltaTime;
            var yTranslation = Input.GetAxis("QE") * Time.deltaTime;
            var zTranslation = Input.GetAxis("Vertical") * Time.deltaTime;
            var translation = new Vector3(xTranslation, yTranslation, zTranslation) * TranslationSpeed;
            transform.Translate(translation, Space.Self);
        }

        private void HandleRotation()
        {
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                Cursor.visible = false;
                var deltaX = Input.mousePosition.x - _lastMouseX;
                if (!Input.GetKeyDown(KeyCode.LeftShift) && !Input.GetKeyDown(KeyCode.RightShift))
                {
                    transform.Rotate(Vector3.up, deltaX * RotationSpeed * Time.deltaTime, Space.World);
                }
                _lastMouseX = Input.mousePosition.x;
            }
            else
            {
                Cursor.visible = true;
            }
        }
    }
}