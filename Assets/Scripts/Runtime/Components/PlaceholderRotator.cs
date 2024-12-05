using System;
using Runtime.Scriptable_Objects;
using UnityEngine;
using DG.Tweening;

namespace Runtime.Components
{
    public class PlaceholderRotator : MonoBehaviour
    {
        [SerializeField] private PlaceHolderBuilder _placeHolderBuilder;
        [SerializeField] private GameObject RDisplay;
        private Vector3 RStartingScale;

        private void OnEnable()
        {
            _placeHolderBuilder.OnSegmentCanRotate += ShowRDisplay;
            _placeHolderBuilder.OnSegmentCannotRotate += HideRDisplay;
            RStartingScale = RDisplay.transform.localScale;
        }

        private void OnDisable()
        {
            _placeHolderBuilder.OnSegmentCanRotate -= ShowRDisplay;
            _placeHolderBuilder.OnSegmentCannotRotate -= HideRDisplay;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                if(RDisplay.activeSelf)
                {
                    var scaleUp = RDisplay.transform.DOScale(RStartingScale * 1.4f, 0.2f).SetEase(Ease.InQuad);
                    var scaleDown = RDisplay.transform.DOScale(RStartingScale * 1f, 0.2f).SetEase(Ease.OutQuad);

                    var sequence = DOTween.Sequence();
                    sequence.Append(scaleUp);
                    sequence.Append(scaleDown);
                }
                
                _placeHolderBuilder.Rotate();
            }
        }

        private void ShowRDisplay()
        {
            RDisplay.SetActive(true);
        }
        
        private void HideRDisplay()
        {
            RDisplay.SetActive(false);
        }
    }
}