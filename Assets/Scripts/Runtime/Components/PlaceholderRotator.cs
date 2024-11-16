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
            if (Input.GetKeyDown(KeyCode.R))
            {
                _placeHolderBuilder.Rotate();
            }
        }
    }
}