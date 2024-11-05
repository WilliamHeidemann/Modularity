using Runtime.Components.Segments;
using Runtime.Scriptable_Objects;
using UnityEngine;
using UnityEngine.Serialization;
using UtilityToolkit.Runtime;

namespace Runtime.Components.Systems
{
    public class GameInitiator : MonoBehaviour
    {
        [SerializeField] private Builder _builder;
        [SerializeField] private Structure _structure;
        [SerializeField] private Hand _hand;
        [SerializeField] private Selection _selection;
        [SerializeField] private Segment _bloodSource;
        [SerializeField] private Segment _steamSource;
        [SerializeField] private Currency _currency;

        private void Start()
        {
            _structure.Clear();
            _hand.Initialize();
            _currency.Initialize();
            _selection.Prefab = Option<Segment>.Some(_bloodSource);
            _builder.Build(new Vector3Int(-3, 0, 3), Quaternion.Euler(180, 0, 0), true);
            _builder.Build(new Vector3Int(3, 3, 3), Quaternion.Euler(180, 0, 0), true);
            _builder.Build(new Vector3Int(-3, 3, -3), Quaternion.Euler(180, 0, 0), true);

            _selection.Prefab = Option<Segment>.Some(_steamSource);
            _builder.Build(new Vector3Int(2, 0, -2), Quaternion.Euler(180, 0, 0), true);
            _builder.Build(new Vector3Int(-3, 3, 3), Quaternion.Euler(180, 0, 0), true);
            _builder.Build(new Vector3Int(0, 10, 0), Quaternion.Euler(0, 0, 0), true);
        }
    }
}