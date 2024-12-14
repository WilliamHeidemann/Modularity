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
            if(costsBlood > 0 && costsSteam > 0)
            {
                _bloodIcon.gameObject.SetActive(true);
                _steamIcon.gameObject.SetActive(true);
                _itemCostBloodText.text = costsBlood.ToString();
                _itemCostSteamText.text = costsSteam.ToString();
                _bluePrintTexture.sprite = _blueprintTextures[2];
            }
            else if(costsBlood > 0)
            {
                _bloodIcon.gameObject.SetActive(true);
                _steamIcon.gameObject.SetActive(false);
                _itemCostBloodText.text = costsBlood.ToString();
                _itemCostSteamText.text = "0";
                _bluePrintTexture.sprite = _blueprintTextures[0];
            }
            else
            {
                _steamIcon.gameObject.SetActive(true);
                _bloodIcon.gameObject.SetActive(false);
                _itemCostSteamText.text = costsSteam.ToString();
                _itemCostBloodText.text = "0";
                _bluePrintTexture.sprite = _blueprintTextures[1];
            }

            if(bloodReward > 0 && steamReward > 0)
            {
                _bloodRewardIcon.gameObject.SetActive(true);
                _steamRewardIcon.gameObject.SetActive(true);
                _itemRewardBloodText.text = bloodReward.ToString();
                _itemRewardSteamText.text = steamReward.ToString();
            }
            else if (bloodReward > 0)
            {
                _bloodRewardIcon.gameObject.SetActive(true);
                _steamRewardIcon.gameObject.SetActive(false);
                _itemRewardBloodText.text = bloodReward.ToString();
            }
            else if (steamReward > 0)
            {
                _steamRewardIcon.gameObject.SetActive(true);
                _bloodRewardIcon.gameObject.SetActive(false);
                _itemRewardSteamText.text = steamReward.ToString();
            }
            else
            {
                _bloodRewardIcon.gameObject.SetActive(false);
                _steamRewardIcon.gameObject.SetActive(false);
            }
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

            if (_itemCostBloodText.text != "0" && _itemCostSteamText.text != "0")
            {
                _glowBlueprints[2].gameObject.SetActive(isGlowing);
            }
            else if (_itemCostBloodText.text != "0")
            {
                _glowBlueprints[0].gameObject.SetActive(isGlowing);
            }
            else
            {
                _glowBlueprints[1].gameObject.SetActive(isGlowing);
            }
        }
    }
}