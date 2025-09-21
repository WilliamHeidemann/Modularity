using Runtime.Scriptable_Objects;
using System.Net.NetworkInformation;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Runtime.Components
{
    public class Blueprint : MonoBehaviour
    {
        [Header("Cost UI")] 
        [SerializeField] private GameObject _costContainer;
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
        [SerializeField] private Image[] _blueprintGradiantBackdrops;
        [SerializeField] private Image[] _glowBlueprints;
        [SerializeField] private Image _itemPreview;

        [Header("Card Animations")]
        [SerializeField] private float _fadeDuration = 0.25f;
        [SerializeField] private float _flyInDistance = 0.25f;
        [SerializeField] private float _flyInDuration = 0.5f;
        private CanvasGroup _canvasGroup;
        [SerializeField] private Vector3 _startingPos;

        private void Start()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
        }

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

            SetGradiantBackdrop(costsBlood, costsSteam);

            _bloodRewardIcon.gameObject.SetActive(bloodReward > 0);
            _steamRewardIcon.gameObject.SetActive(steamReward > 0);
            _itemRewardBloodText.text = bloodReward.ToString();
            _itemRewardSteamText.text = steamReward.ToString();
        }

        public void PlayCardIntroAnimation()
        {
            Vector3 currentPosition = _startingPos;
            Vector3 startPosition = new Vector3(currentPosition.x + Screen.width * _flyInDistance, currentPosition.y, currentPosition.z);
            transform.localPosition = startPosition;
            _canvasGroup.alpha = 0;
            _canvasGroup.DOFade(1, _fadeDuration);
            transform.DOLocalMoveX(currentPosition.x, _flyInDuration).SetEase(Ease.OutBack).OnComplete(() =>
            {
                transform.localPosition = currentPosition;
                _canvasGroup.alpha = 1;
            });
        }

        public void PlayCardOutroAnimation()
        {
            // Animate the card flying out to the right while fading out over time, then reset its position and keep it invisible
            Vector3 currentPosition = _startingPos;
            Vector3 endPosition = new Vector3(currentPosition.x + Screen.width * _flyInDistance, currentPosition.y, currentPosition.z);
            _canvasGroup.DOFade(0, _fadeDuration);
            transform.DOLocalMoveX(endPosition.x, _flyInDuration).SetEase(Ease.OutBack).OnComplete(() =>
            {
                transform.localPosition = currentPosition;
                _canvasGroup.alpha = 0;
            });
        }

        public void PlayPickCardAnimation()
        {
            //animate the card flying rapidly towards the left and fading out
            Vector3 currentPosition = _startingPos;
            Vector3 endPosition = new Vector3(currentPosition.x - Screen.width, currentPosition.y, currentPosition.z);
            _canvasGroup.DOFade(0, _fadeDuration / 6);
            transform.DOLocalMoveX(endPosition.x, _flyInDuration / 1.5f).SetEase(Ease.Linear).OnComplete(() =>
            {
                transform.localPosition = currentPosition;
                _canvasGroup.alpha = 0;
            });
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

        public void SetGradiantBackdrop(int costBlood, int costSteam)
        {
            var gradiantObject = (costBlood != 0, costSteam != 0) switch
            {
                (true, true) => _blueprintGradiantBackdrops[2],
                (true, false) => _blueprintGradiantBackdrops[0],
                (false, true) => _blueprintGradiantBackdrops[1],
                _ => _blueprintGradiantBackdrops[0]
            };

            foreach (var gradiant in _blueprintGradiantBackdrops)
            {
                gradiant.gameObject.SetActive(gradiant == gradiantObject);
            }
        }
        
        public void SetCostVisible(bool isVisible)
        {
            _costContainer.SetActive(isVisible);
        }
    }
}