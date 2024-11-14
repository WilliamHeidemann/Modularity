using Runtime.Components.Segments;
using Runtime.Components.Utility;
using Runtime.Scriptable_Objects;
using UnityEngine;
using UnityUtils;
using UtilityToolkit.Runtime;

namespace Runtime.Components.Systems
{
    public class GameInitiator : MonoBehaviour
    {
        [SerializeField] private Builder _builder;
        [SerializeField] private Structure _structure;
        [SerializeField] private Hand _hand;
        [SerializeField] private FlowControl _flowControl;
        [SerializeField] private Selection _selection;
        [SerializeField] private Segment _bloodSource;
        [SerializeField] private Segment _steamSource;
        [SerializeField] private Currency _currency;
        [SerializeField] private int _startingCurrency;
        
        [SerializeField] private Transform[] _startingBloodPoints;
        [SerializeField] private Transform[] _startingSteamPoints;

        private void Start()
        {
            _structure.Clear();
            _flowControl.Clear();
            _hand.Initialize();
            _currency.Initialize(_startingCurrency);
            _selection.Prefab = Option<Segment>.Some(_bloodSource);
            _startingBloodPoints.ForEach(point => 
                _builder.Build(point.position.AsVector3Int(), Quaternion.Euler(180, 0, 0), true));

            _selection.Prefab = Option<Segment>.Some(_steamSource);
            _startingSteamPoints.ForEach(point => 
                _builder.Build(point.position.AsVector3Int(), Quaternion.Euler(180, 0, 0), true));

            _selection.Prefab = Option<Segment>.None;
        }
    }
}