using Runtime.Components.Segments;
using UnityEngine;
using UnityEngine.Serialization;
using System.Collections.Generic;
using Runtime.Backend;
using Runtime.Components.Systems;
using Runtime.Components.Utility;
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
        private bool _onlyGenerateBloodSegments;

        //the segments that the player can choose from
        public List<Segment> SegmentsOptions;

        private const int OptionsCount = 3;

        public void Initialize()
        {
            _onlyGenerateBloodSegments = true;
            GenerateHand();
        }

        public void SelectBlueprint(int chosenSegment)
        {
            _selection.Prefab = Option<Segment>.Some(SegmentsOptions[chosenSegment]);
            _selection.PriceBlood = SegmentsOptions[chosenSegment].StaticSegmentData.BloodCost;
            _selection.PriceSteam = SegmentsOptions[chosenSegment].StaticSegmentData.SteamCost;
            SoundFXPlayer.Instance.Play(SoundFX.CardSelection);
        }

        public void GenerateHand()
        {
            SegmentsOptions = new List<Segment>();
            for (int i = 0; i < OptionsCount; i++)
            {
                var segment = SpawnUtility.Get(_pool.GetRandomSegment, IsValid);
                SegmentsOptions.Add(segment);
            }

            OnDrawHand?.Invoke();
        }

        public void EnableSteamSegments()
        {
            _onlyGenerateBloodSegments = false;
        }

        private bool IsValid(Segment segment)
        {
            if (SegmentsOptions.Contains(segment))
            {
                return false;
            }

            if (_onlyGenerateBloodSegments && segment.StaticSegmentData.IsSteam)
            {
                return false;
            }

            return true;
        }
    }
}