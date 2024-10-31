using Runtime.Components.Segments;
using Runtime.Scriptable_Objects;
using UnityEngine;
using UtilityToolkit.Runtime;

namespace Runtime.Components.Systems
{
    public class GameInitiator : MonoBehaviour
    {
        [SerializeField] private Transform _sceneCanvas;
        [SerializeField] private Builder _builder;
        [SerializeField] private Structure _structure;
        [SerializeField] private Hand _hand;
        [SerializeField] private Selection _selection;
        [SerializeField] private Segment _startingSegment;
        
        private void Start()
        {
            _structure.Clear();
            _hand.Initialize();
            _selection.Prefab = Option<Segment>.Some(_startingSegment);
            _builder.Build(Vector3Int.zero, Quaternion.Euler(180, 0, 0));
        }
    }
}