using System;
using System.Linq;
using Runtime.DataLayer;
using UnityEngine;

namespace Runtime.Components.Utility
{
    public class DeveloperInputReader : MonoBehaviour
    {
        [SerializeField] private Structure _structure;
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SaveStructure();
            }
        }

        private void SaveStructure()
        {
            string json = _structure.ToJson();
            print($"RAW JSON STRING: \n{json}");
            print($"JSON -> structure -> JSON: \n{json.ToStructure().ToJson()}");
        }
    }
}
