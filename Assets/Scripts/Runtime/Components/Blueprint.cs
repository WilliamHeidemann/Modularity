using Runtime.Scriptable_Objects;
using System.Net.NetworkInformation;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.Components
{
    public class Blueprint : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _itemCostBlood;
        [SerializeField] private TextMeshProUGUI _itemCostSteam;
        [SerializeField] private Image _bloodIcon;
        [SerializeField] private Image _steamIcon;
        [SerializeField] private Image _itemPreview;
        [SerializeField] private Image _glowBlueprint;
        
        public void SetCost(string itemCost, int costsBlood, int costsSteam)
        {
            if(costsBlood > 0 && costsSteam > 0)
            {
                _bloodIcon.gameObject.SetActive(true);
                _steamIcon.gameObject.SetActive(true);
                _itemCostBlood.text = itemCost;
                _itemCostSteam.text = itemCost;
            }
            else if(costsBlood > 0)
            {
                _bloodIcon.gameObject.SetActive(true);
                _steamIcon.gameObject.SetActive(false);
                _itemCostBlood.text = itemCost;
            }
            else
            {
                _steamIcon.gameObject.SetActive(true);
                _bloodIcon.gameObject.SetActive(false);
                _itemCostSteam.text = itemCost;
            }
        }

        public void SetPreview(Sprite itemPreview)
        {
            _itemPreview.sprite = itemPreview;
        }

        public void GlowState(bool isGlowing)
        {
            if(isGlowing)
            {
                _glowBlueprint.gameObject.SetActive(true);
            }
            else
            {
                _glowBlueprint.gameObject.SetActive(false);
            }
        }
    }
}