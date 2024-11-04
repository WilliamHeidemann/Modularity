using System;
using Runtime.Scriptable_Objects;
using UnityEngine;

namespace Runtime.Components
{
    public class PlaceholderRotator : MonoBehaviour
    {
        [SerializeField] private PlaceHolderBuilder _placeHolderBuilder;

        private void Update()
        {
            // if (Input.GetKeyDown(KeyCode.E))
            // {
            //     _placeHolderBuilder.RotateOnY();
            // }
            //
            // if (Input.GetKeyDown(KeyCode.Q))
            // {
            //     _placeHolderBuilder.RotateOnX();
            // }

            if (Input.GetKeyDown(KeyCode.R))
            {
                _placeHolderBuilder.Rotate();
            }
        }
    }
}