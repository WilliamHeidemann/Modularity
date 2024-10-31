using Runtime.Components.Segments;
using UnityEngine;
using UnityEngine.Serialization;
using System.Collections.Generic;
using UtilityToolkit.Runtime;
using Codice.CM.Client.Differences.Merge;

namespace Runtime.Scriptable_Objects
{
    [CreateAssetMenu]
    public class Hand : ScriptableObject
    {
        [SerializeField] private Selection _selection;

        //the segments that the player can choose from
        public Segment[] _segmentsOptions;
        [SerializeField] private List<Segment> _pool;

        public int _optionsCount = 3;

        public void Awake()
        {
            _segmentsOptions = new Segment[3];
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
                _segmentsOptions[i] = _pool.RandomElement();
            }
        }
    }
}