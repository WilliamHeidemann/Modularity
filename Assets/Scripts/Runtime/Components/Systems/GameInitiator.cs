using Runtime.Scriptable_Objects;
using UnityEngine;

namespace Runtime.Components.Systems
{
    public class GameInitiator : MonoBehaviour
    {
        [SerializeField] private Transform _sceneCanvas;
        [SerializeField] private Builder _builder;

        private void Start()
        {
            _builder.Build(Vector3Int.zero);
        }
    }
}