using System;
using System.Collections.Generic;
using System.IO;
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
        [SerializeField] private Structure _structure;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.J)) SaveStructureToJson();
            if (Input.GetKeyDown(KeyCode.L)) LoadStructureFromJson();
            if (Input.GetKeyDown(KeyCode.M)) CreateMegaStructure();
        }

        private void CreateMegaStructure()
        {
            var segments = CreateStructure();
            foreach (SegmentData segmentData in segments)
            {
                _builder.BuildInstant(segmentData, _treePrefab);
            }
        }

        private void TearDownStructure()
        {
            var segments = FindObjectsByType<Segment>(FindObjectsSortMode.None);
            for (int i = 0; i < segments.Length; i++)
            {
                Destroy(segments[i].gameObject);
            }
            _structure.Clear();
        }

        private IEnumerable<SegmentData> CreateStructure()
        {
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

        private void SaveStructureToJson()
        {
            string json = _structure.ToJson();
            var filePath = $"{Application.persistentDataPath}/structure.txt";
        
            try
            {
                using var writer = new StreamWriter(filePath);
                Debug.Log($"File saved to {filePath}");
                Debug.Log(json);
                writer.WriteLine(json);
            }
            catch (System.Exception ex)
            {
                Debug.LogError("Error writing to file: " + ex.Message);
            }
        }

        public void LoadStructureFromJson()
        {
            TearDownStructure();

            var segments = Resources.FindObjectsOfTypeAll<Segment>();
            
            var filePath = $"{Application.persistentDataPath}/structure.txt";
            if (!File.Exists(filePath))
            {
                Debug.LogError("File does not exist");
                return;
            }

            try
            {
                using var reader = new StreamReader(filePath);
                string json = reader.ReadToEnd();
                var structure = json.ToStructure();
                _structure.Clear();
                foreach (SegmentData segmentData in structure.Segments)
                {
                    var prefabOption = segments.FirstOption(s => s.StaticSegmentData == segmentData.StaticSegmentData);
                    if (prefabOption.IsSome(out Segment prefab))
                    {
                        _builder.BuildInstant(segmentData, prefab);
                    }
                    else
                    {
                        Debug.LogError($"Prefab not found for static segment data {segmentData.StaticSegmentData}");
                    }
                }
                Debug.Log("Structure loaded from file");
                Debug.Log(json);
            }
            catch (System.Exception ex)
            {
                Debug.LogError("Error reading from file: " + ex.Message);
            }
        }
    }
}