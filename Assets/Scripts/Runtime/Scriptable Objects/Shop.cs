using Runtime.Components.Segments;
using UnityEngine;
using UnityEngine.Serialization;
using System.Collections.Generic;
using UtilityToolkit.Runtime;
using Codice.CM.Client.Differences.Merge;

namespace Runtime.Scriptable_Objects
{
    [CreateAssetMenu]
    public class Shop : ScriptableObject
    {
        // Contains information of what the player can choose between
        // as well as the cost for each option.

        // Has a bank of prefabs to select from with various probabilities 
        // for each segment, that are easily modified by a designer. 

        // When the player has clicked the UI element representing their 
        // chosen prefab, and the player has enough resources, the 
        // selected segment will change to this segment prefab.

        [SerializeField] private Selection _selection;
        [SerializeField] private Resources _resources;

        //the segments that the player can choose from
        public Segment[] _segmentsOptions;
        [SerializeField] private List<Segment> _availableSegments;

        public int _optionsCount = 3;

        public void Awake()
        {
            _segmentsOptions = new Segment[3];
            GenerateOptions();
        }

        public void SelectSegment(int choosenSegment)
        {
            Debug.Log("Selected segment: " + choosenSegment);
            Debug.Log(_segmentsOptions[choosenSegment]);
            _selection.Prefab = _segmentsOptions[choosenSegment];
            _selection.Price = _segmentsOptions[choosenSegment].ConnectionPoints.OpenConnectionPoints();
        }

        public void GenerateOptions()
        {
            for(int i = 0; i < _optionsCount; i++)
            {
                _segmentsOptions[i] = _availableSegments.RandomElement();
            }
        }
    }
}