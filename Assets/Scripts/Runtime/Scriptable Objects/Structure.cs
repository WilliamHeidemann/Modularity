using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

namespace Runtime.Scriptable_Objects
{
    [CreateAssetMenu]
    public class Structure : ScriptableObject
    {
        public readonly HashSet<Vector3Int> SegmentPositions = new();
        public readonly HashSet<Vector3Int> SlotPositions = new();
        public HashSet<Vector3Int> TakenPositions => SegmentPositions.Concat(SlotPositions).ToHashSet();
        private Dictionary<Vector3Int, SegmentData> GraphData;

        public void AddSegment(Vector3Int position, IEnumerable<Vector3Int> connections)
        {
            SegmentData segmentData = new()
            {
                Connections = connections.ToHashSet()
            };
            GraphData.Add(position,segmentData);

            foreach (var resident in connections)
            {
                AddNeighbour(resident, position);
            }
        }
        public void AddNeighbour(Vector3Int resident, Vector3Int neighbour){
            GraphData[resident].Connections.Add(neighbour);
        }

        public void UpdateFlow(SegmentData segmentData)
        {
            throw new NotImplementedException();
        }
    }

    public struct SegmentData{
        public HashSet<Vector3Int> Connections;
        public int Blood;
        public int Steam;
    }

}