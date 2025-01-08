using System;
using System.Collections.Generic;
using System.Linq;
using Runtime.Components.Segments;
using Runtime.DataLayer;
using Runtime.Scriptable_Objects;
using UnityEngine;
using UtilityToolkit.Runtime;

namespace Runtime.Components.Utility
{
    public class DeveloperInputReader : MonoBehaviour
    {
        [SerializeField] private Builder _builder;
        [SerializeField] private Segment _treePrefab;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                var segments = CreateStructure();
                foreach (SegmentData segmentData in segments)
                {
                    _builder.BuildInstant(segmentData, _treePrefab);
                }
                // SaveStructure();
            }
        }

        private IEnumerable<SegmentData> CreateStructure()
        {
            var structure = ScriptableObject.CreateInstance<Structure>();
            
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    for (int k = 0; k < 10; k++)
                    {
                        var segment = new SegmentData
                        {
                            Position = new Vector3Int(i, j, k),
                            StaticSegmentData = _treePrefab.StaticSegmentData
                        };
                        yield return segment;
                    }
                }
            }
        }

        // private void SaveStructure()
        // {
        //     string json = _structure.ToJson();
        //     print($"RAW JSON STRING: \n{json}");
        //     print($"JSON -> structure -> JSON: \n{json.ToStructure().ToJson()}");
        // }
    }
}