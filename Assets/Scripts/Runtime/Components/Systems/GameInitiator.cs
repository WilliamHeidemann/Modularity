using Runtime.Scriptable_Objects;
using UnityEngine;
using UnityEngine.XR;

namespace Runtime.Components.Systems
{
    public class GameInitiator : MonoBehaviour
    {
        [SerializeField] private Transform _sceneCanvas;
        [SerializeField] private Builder _builder;
        [SerializeField] private Structure _structure;

        private void Start()
        {
            _structure.Clear();
            _builder.Build(Vector3Int.zero, Quaternion.identity);
        }
    }
}