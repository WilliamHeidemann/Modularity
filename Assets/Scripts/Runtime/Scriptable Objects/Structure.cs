using System;
using System.Collections.Generic;
using System.Linq;
using Codice.CM.Client.Differences.Merge;
using GluonGui.Dialog;
using Runtime.Components.Utility;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;
using UnityUtils;

namespace Runtime.Scriptable_Objects
{
    [CreateAssetMenu]
    public class Structure : ScriptableObject
    {
        //I think maybe SegmentPositions has been made redundant by _graphData.keys
        public readonly HashSet<Vector3Int> SegmentPositions = new();
        public readonly HashSet<Vector3Int> SlotPositions = new();
        public HashSet<Vector3Int> TakenPositions => SegmentPositions.Concat(SlotPositions).ToHashSet();
        private readonly Dictionary<Vector3Int, SegmentData> _graphData = new();

        public void AddSegment(Vector3Int position, StaticSegmentData ssData, HashSet<Vector3Int> connections)
        {
            //Creating and filling out the segmentData
            SegmentData segmentData = new() {ConnectorSpots = connections};

            foreach (var connection in connections)
            {
                if (_graphData.Keys.Contains(connection))
                {
                    segmentData.Connections.Add(connection);
                }
            }
            
            _graphData.Add(position, segmentData);
            
            //informing neighbours og slot occupation
            segmentData.Connections.ForEach(connection => _graphData[connection].Connections.Add(position));
            
            //UpdateFlow(segmentData);
        }

        public void UpdateFlow(SegmentData segmentData)
        {
            throw new NotImplementedException();
        }

        public bool CanConnect(Vector3Int position, HashSet<Vector3Int> connections)
        {
            //Hack for placing first segment CHANGE !!
            if (position == Vector3Int.zero) return true;


            bool result = false;
            foreach (var connection in connections)
            {
                if (_graphData.Keys.Contains(connection))
                {
                    result = result || CanConnectInner(position, _graphData[connection]);
                }
            }
            
            return result;
        }

        public bool CanConnectInner(Vector3Int position, SegmentData target, bool blood = false, bool steam = false)
        {
            //to be implemented blood and steam
            //if (!(blood && target.TransfersBlood || steam && target.TransfersSteam)) return false;
            if (!target.ConnectorSpots.Contains(position)) return false;
            
            return true;
        }

    }

    public class SegmentData
    {
        public HashSet<Vector3Int> Connections = new();
        public HashSet<Vector3Int> ConnectorSpots = new();
        public bool TransfersBlood;
        public bool TransfersSteam;
        public int Blood;
        public int Steam;
        public int Resistance;
    }
}