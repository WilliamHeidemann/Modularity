using Runtime.Components.Segments;
using UnityEngine;
using UnityEngine.Serialization;
using System.Collections.Generic;
using UtilityToolkit.Runtime;
using Codice.CM.Client.Differences.Merge;
using Runtime.Components.Systems;

namespace Runtime.Scriptable_Objects
{
    [CreateAssetMenu]
    public class Hand : ScriptableObject
    {
        public delegate void DrawHand();
        public static event DrawHand OnDrawHand;

        [SerializeField] private Selection _selection;

        //the segments that the player can choose from
        public Segment[] _segmentsOptions;
        [SerializeField] private List<Segment> _pool; //needs to be assigned from the inspector

        public int _optionsCount = 3;

        public void Initialize()
        {
            _segmentsOptions = new Segment[_optionsCount];
            GenerateHand();
        }

        public void SelectSegment(int chosenSegment)
        {
            _selection.Prefab = _segmentsOptions[chosenSegment];
            _selection.Price = _segmentsOptions[chosenSegment].ConnectionPoints.OpenConnectionPoints();

            //generates next hand
            GenerateHand();
        }

        public void GenerateHand()
        {
            for(int i = 0; i < _optionsCount; i++)
            {
                _segmentsOptions[i] = _pool.RandomElement();
            }
            OnDrawHand?.Invoke();
        }
    }
}