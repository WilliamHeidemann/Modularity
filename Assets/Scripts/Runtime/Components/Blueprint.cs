using Runtime.Scriptable_Objects;
using System.Net.NetworkInformation;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.Components
{
    public class Blueprint : MonoBehaviour
    {
        [Header("Cost UI")]
        [SerializeField] private TextMeshProUGUI _itemCostBloodText;
        [SerializeField] private TextMeshProUGUI _itemCostSteamText;
        [SerializeField] private Image _bloodIcon;
        [SerializeField] private Image _steamIcon;

        [Header("Reward UI")]
        [SerializeField] private TextMeshProUGUI _itemRewardBloodText;
        [SerializeField] private TextMeshProUGUI _itemRewardSteamText;
        [SerializeField] private Image _bloodRewardIcon;
        [SerializeField] private Image _steamRewardIcon;

        [Header("Blueprint UI")]
        [SerializeField] private Image _bluePrintTexture;
        [SerializeField] private Sprite[] _blueprintTextures;
        [SerializeField] private Image[] _glowBlueprints;
        [SerializeField] private Image _itemPreview;

        public void SetCardValues(int costsBlood, int costsSteam, int bloodReward, int steamReward)
        {
            _bloodIcon.gameObject.SetActive(costsBlood > 0);
            _steamIcon.gameObject.SetActive(costsSteam > 0);
            _itemCostBloodText.text = costsBlood.ToString();
            _itemCostSteamText.text = costsSteam.ToString();
            _bluePrintTexture.sprite = (costsBlood > 0, costsSteam > 0) switch
            {
                (true, true) => _blueprintTextures[2],
                (true, false) => _blueprintTextures[0],
                (false, true) => _blueprintTextures[1],
                _ => _blueprintTextures[0]
            };

            _bloodRewardIcon.gameObject.SetActive(bloodReward > 0);
            _steamRewardIcon.gameObject.SetActive(steamReward > 0);
            _itemRewardBloodText.text = bloodReward.ToString();
            _itemRewardSteamText.text = steamReward.ToString();
        }

        public void SetPreview(Sprite itemPreview)
        {
            _itemPreview.sprite = itemPreview;
        }

        public void GlowState(bool isGlowing)
        {
            if (!isGlowing)
            {
                foreach (var glow in _glowBlueprints)
                {
                    glow.gameObject.SetActive(isGlowing);
                }
                return;
            }

            var glowObject = (_itemCostBloodText.text != "0", _itemCostSteamText.text != "0") switch
            {
                (true, true) => _glowBlueprints[2].gameObject,
                (true, false) => _glowBlueprints[0].gameObject,
                (false, true) => _glowBlueprints[1].gameObject,
                _ => _glowBlueprints[0].gameObject
            };
            
            glowObject.SetActive(isGlowing);
        }
    }
}