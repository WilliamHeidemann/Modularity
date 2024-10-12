using UnityEngine;

namespace Runtime.Components
{
    public class Billboard : MonoBehaviour
    {
        private Camera _camera;

        private void Start()
        {
            _camera = Camera.main;
        }

        private void Update()
        {
            var cameraPosition = _camera.transform.position;
            cameraPosition.y = transform.position.y;
            transform.LookAt(cameraPosition);
        }
    }
}
