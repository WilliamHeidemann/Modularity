using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityUtils;

namespace Runtime.Scriptable_Objects
{
    [CreateAssetMenu]
    public class Structure : ScriptableObject
    {
        public readonly HashSet<Vector3Int> SegmentPositions = new();
        public readonly HashSet<Vector3Int> SlotPositions = new();
        public HashSet<Vector3Int> TakenPositions => SegmentPositions.Concat(SlotPositions).ToHashSet();
        private readonly Dictionary<Vector3Int, SegmentData> _graphData = new();

        public void AddSegment(Vector3Int position, HashSet<Vector3Int> connections)
        {
            SegmentData segmentData = new()
            {
                Connections = connections
            };
            _graphData.Add(position, segmentData);

            connections.ForEach(connection => _graphData[connection].Connections.Add(position));
            
            UpdateFlow(segmentData);
        }

        public void UpdateFlow(SegmentData segmentData)
        {
            throw new NotImplementedException();
        }
    }

    public class SegmentData
    {
        public HashSet<Vector3Int> Connections;
        public bool TransfersBlood;
        public bool TransfersSteam;
        public int Blood;
        public int Steam;
    }
}