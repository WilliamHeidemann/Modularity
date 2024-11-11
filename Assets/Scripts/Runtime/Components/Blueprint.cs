using Runtime.Scriptable_Objects;
using System.Net.NetworkInformation;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.Components
{
    public class Blueprint : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _itemCost;
        [SerializeField] private Image _itemPreview;
        
        public void SetCost(string itemCost)
        {
            _itemCost.text = itemCost;
        }

        public void SetPreview(Sprite itemPreview)
        {
            _itemPreview.sprite = itemPreview;
        }
    }
}