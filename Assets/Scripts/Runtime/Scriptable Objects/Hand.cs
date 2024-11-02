using Runtime.Components.Segments;
using UnityEngine;
using UnityEngine.Serialization;
using System.Collections.Generic;
using Codice.CM.Client.Differences.Merge;
using Runtime.Components.Systems;
using UtilityToolkit.Runtime;

namespace Runtime.Scriptable_Objects
{
    [CreateAssetMenu]
    public class Hand : ScriptableObject
    {
        public delegate void DrawHand();
        public event DrawHand OnDrawHand;

        [SerializeField] private Selection _selection;
        [SerializeField] private SegmentPool _pool;

        //the segments that the player can choose from
        public Segment[] _segmentsOptions;

        public int _optionsCount = 3;

        public void Initialize()
        {
            _segmentsOptions = new Segment[_optionsCount];
            GenerateHand();
        }

        public void SelectBlueprint(int chosenSegment)
        {
            _selection.Prefab = Option<Segment>.Some(_segmentsOptions[chosenSegment]);
            _selection.Price = _segmentsOptions[chosenSegment].StaticSegmentData.ConnectionPoints.OpenConnectionPoints();
        }

        public void GenerateHand()
        {
            for(int i = 0; i < _optionsCount; i++)
            {
                Segment segment = _pool.GetRandomSegment();
                _segmentsOptions[i] = segment;
            }
            OnDrawHand?.Invoke();
        }
    }
}