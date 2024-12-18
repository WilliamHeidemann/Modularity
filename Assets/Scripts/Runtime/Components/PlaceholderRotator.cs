using System;
using Runtime.Scriptable_Objects;
using UnityEngine;
using DG.Tweening;

namespace Runtime.Components
{
    public class PlaceholderRotator : MonoBehaviour
    {
        [SerializeField] private PlaceHolderBuilder _placeHolderBuilder;
        [SerializeField] private GameObject _rDisplay;
        [SerializeField] private Transform _rotationPoint;
        private Vector3 RStartingScale;

        private void OnEnable()
        {
            _placeHolderBuilder.OnSegmentCanRotate += ShowRDisplay;
            _placeHolderBuilder.OnSegmentCannotRotate += HideRDisplay;
            RStartingScale = _rDisplay.transform.localScale;
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
                if(_rDisplay.activeSelf)
                {
                    var scaleUp = _rDisplay.transform.DOScale(RStartingScale * 1.4f, 0.2f).SetEase(Ease.InQuad);
                    var scaleDown = _rDisplay.transform.DOScale(RStartingScale * 1f, 0.2f).SetEase(Ease.OutQuad);
                    var Rotate = _rotationPoint.DORotate(_rotationPoint.rotation * new Vector3(0, 0, 180), 0.3f).SetEase(Ease.InOutQuad);

                    var sequence = DOTween.Sequence();
                    sequence.Append(scaleUp);
                    sequence.Append(scaleDown);

                    var sequence2 = DOTween.Sequence().Append(Rotate);
                    sequence2.OnComplete(() =>
                    {
                        _rotationPoint.rotation = Quaternion.Euler(0, 0, 0);
                    });
                }
                
                _placeHolderBuilder.Rotate();
            }
        }

        private void ShowRDisplay()
        {
            _rDisplay.SetActive(true);
        }
        
        private void HideRDisplay()
        {
            _rDisplay.SetActive(false);
        }
    }
}