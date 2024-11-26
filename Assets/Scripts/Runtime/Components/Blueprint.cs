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

        [Header("Input UI")]
        [SerializeField] private TextMeshProUGUI _itemInputBloodText;
        [SerializeField] private TextMeshProUGUI _itemInputSteamText;
        [SerializeField] private Image _bloodInputIcon;
        [SerializeField] private Image _steamInputIcon;

        [Header("Output UI")]
        [SerializeField] private TextMeshProUGUI _itemOutputText;
        [SerializeField] private Image _outputIcon;

        [Header("Blueprint UI")]
        [SerializeField] private Image _bluePrintTexture;
        [SerializeField] private Sprite[] _blueprintTextures;
        [SerializeField] private Image[] _glowBlueprints;
        [SerializeField] private Image _itemPreview;

        public void SetCardValues(string itemCost, int costsBlood, int costsSteam, int bloodInputs, int steamInputs, int rewardAmount)
        {
            if(costsBlood > 0 && costsSteam > 0)
            {
                _bloodIcon.gameObject.SetActive(true);
                _steamIcon.gameObject.SetActive(true);
                _itemCostBloodText.text = itemCost;
                _itemCostSteamText.text = itemCost;
                _bluePrintTexture.sprite = _blueprintTextures[2];
            }
            else if(costsBlood > 0)
            {
                _bloodIcon.gameObject.SetActive(true);
                _steamIcon.gameObject.SetActive(false);
                _itemCostBloodText.text = itemCost;
                _itemCostSteamText.text = "0";
                _bluePrintTexture.sprite = _blueprintTextures[0];
            }
            else
            {
                _steamIcon.gameObject.SetActive(true);
                _bloodIcon.gameObject.SetActive(false);
                _itemCostSteamText.text = itemCost;
                _itemCostBloodText.text = "0";
                _bluePrintTexture.sprite = _blueprintTextures[1];
            }

            if(bloodInputs > 0 && steamInputs > 0)
            {
                _bloodInputIcon.gameObject.SetActive(true);
                _steamInputIcon.gameObject.SetActive(true);
                _itemInputBloodText.text = bloodInputs.ToString();
                _itemInputSteamText.text = steamInputs.ToString();
            }
            else if (bloodInputs > 0)
            {
                _bloodInputIcon.gameObject.SetActive(true);
                _steamInputIcon.gameObject.SetActive(false);
                _itemInputBloodText.text = bloodInputs.ToString();
            }
            else if (steamInputs > 0)
            {
                _steamInputIcon.gameObject.SetActive(true);
                _bloodInputIcon.gameObject.SetActive(false);
                _itemInputSteamText.text = steamInputs.ToString();
            }
            else
            {
                _bloodInputIcon.gameObject.SetActive(false);
                _steamInputIcon.gameObject.SetActive(false);
            }

            if (rewardAmount > 0)
            {
                _outputIcon.gameObject.SetActive(true);
                _itemOutputText.text = rewardAmount.ToString();
            }
            else
            {
                _outputIcon.gameObject.SetActive(false);
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