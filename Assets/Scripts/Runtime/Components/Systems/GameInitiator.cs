using Runtime.Components.Segments;
using Runtime.Scriptable_Objects;
using UnityEngine;
using UtilityToolkit.Runtime;

namespace Runtime.Components.Systems
{
    public class GameInitiator : MonoBehaviour
    {
        [SerializeField] private Builder _builder;
        [SerializeField] private Structure _structure;
        [SerializeField] private Hand _hand;
        [SerializeField] private Selection _selection;
        [SerializeField] private Segment _startingSegment;
        [SerializeField] private Segment _startingSegment2;
        [SerializeField] private Currency _currency;
        
        private void Start()
        {
            _structure.Clear();
            _hand.Initialize();
            _currency.Initialize();
            _selection.Prefab = Option<Segment>.Some(_startingSegment);
            _builder.Build(Vector3Int.zero, Quaternion.Euler(180, 0, 0), true);

            _selection.Prefab = Option<Segment>.Some(_startingSegment2);
            _builder.Build(new Vector3Int(1,0,0), Quaternion.Euler(180, 0, 0), true);
        }
    }
}