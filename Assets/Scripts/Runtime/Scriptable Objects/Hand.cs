using Runtime.Components.Segments;
using UnityEngine;
using UnityEngine.Serialization;
using System.Collections.Generic;
using Codice.CM.Client.Differences.Merge;

namespace Runtime.Scriptable_Objects
{
    [CreateAssetMenu]
    public class Hand : ScriptableObject
    {
        [SerializeField] private Selection _selection;
        [SerializeField] private SegmentPool _pool;

        //the segments that the player can choose from
        public Segment[] _segmentsOptions;

        public int _optionsCount = 3;

        public void Awake()
        {
            _segmentsOptions = new Segment[_optionsCount];
            GenerateOptions();
        }

        public void SelectSegment(int chosenSegment)
        {
            _selection.Prefab = _segmentsOptions[chosenSegment];
            _selection.Price = _segmentsOptions[chosenSegment].StaticSegmentData.ConnectionPoints.OpenConnectionPoints();
        }

        public void GenerateOptions()
        {
            for(int i = 0; i < _optionsCount; i++)
            {
                Segment segment = _pool.GetRandomSegment();
                _segmentsOptions[i] = segment;
                Debug.Log("Choosen segment. " + segment);
            }
        }
    }
}