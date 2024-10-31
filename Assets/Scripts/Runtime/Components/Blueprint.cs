using Runtime.Scriptable_Objects;
using System.Net.NetworkInformation;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.Components
{
    public class Blueprint : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _itemName;
        [SerializeField] private TextMeshProUGUI _itemCost;

        public void SetName(string itemName)
        {
            _itemName.text = itemName;
        }
        
        public void SetCost(string itemCost)
        {
            _itemCost.text = itemCost;
        }
    }
}