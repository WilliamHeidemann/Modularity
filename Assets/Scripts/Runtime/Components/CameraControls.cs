using UnityEngine;

namespace Runtime.Components
{
    public class CameraControls : MonoBehaviour
    {
        private const float RotationSpeed = 100f;
    
        private void Update()
        {
            HandleMovement();
            HandleRotation();
        }

        private void HandleMovement()
        {
            var distanceToOrigin = Vector3.Distance(transform.position, Vector3.zero);

            var input = Input.GetAxis("Vertical");

            if (distanceToOrigin < 3 && input > 0)
            {
                return;
            }

            if (distanceToOrigin > 20 && input < 0)
            {
                return;
            }

            var translation = input * Time.deltaTime * (Vector3.zero - transform.position);
            transform.Translate(translation, Space.World);
        }

        private void HandleRotation()
        {
            var yRotation = Input.GetAxis("Horizontal") * Time.deltaTime * RotationSpeed;
            transform.RotateAround(Vector3.zero, Vector3.down, yRotation);
        }
    }
}
