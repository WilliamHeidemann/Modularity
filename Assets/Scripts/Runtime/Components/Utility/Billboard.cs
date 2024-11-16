using UnityEngine;

namespace Runtime.Components.Utility
{
    public class Billboard : MonoBehaviour
    {
        private Transform _mainCamera;

        private void Start()
        {
            _mainCamera = Camera.main.transform;
        }

        private void LateUpdate()
        {
            transform.rotation = Quaternion.LookRotation(transform.position - _mainCamera.position);
        }
    }
}