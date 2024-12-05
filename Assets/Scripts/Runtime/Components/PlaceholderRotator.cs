using System;
using Runtime.Scriptable_Objects;
using UnityEngine;

namespace Runtime.Components
{
    public class PlaceholderRotator : MonoBehaviour
    {
        [SerializeField] private PlaceHolderBuilder _placeHolderBuilder;
        [SerializeField] private GameObject RDisplay;

        private void OnEnable()
        {
            _placeHolderBuilder.OnSegmentCanRotate += ShowRDisplay;
            _placeHolderBuilder.OnSegmentCannotRotate += HideRDisplay;
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