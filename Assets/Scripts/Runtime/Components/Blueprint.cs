using Runtime.Scriptable_Objects;
using System.Net.NetworkInformation;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.Components
{
    public class Blueprint : MonoBehaviour
    {
        [SerializeField] private Hand _shop;

        [SerializeField] private TextMeshProUGUI _itemName;
        [SerializeField] private TextMeshProUGUI _itemCost;
        [SerializeField] private Button _button;

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