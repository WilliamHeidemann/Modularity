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
        [SerializeField] private Scriptable_Objects.Hand _hand;

        private void Start()
        {
            _structure.Clear();
            _hand.Initialize();
            _builder.Build(Vector3Int.zero, Quaternion.identity);
        }
    }
}