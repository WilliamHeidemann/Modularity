using System;
using Runtime.Models;
using UnityEngine;

namespace Runtime.Components.Gameplay
{
    public class ConnectionSlot : MonoSegment
    {
        [SerializeField] private GameObject _placeHolder;
        public static event Action<Position> OnSlotClicked;
        
        private void OnMouseEnter()
        {
            _placeHolder.SetActive(true);
        }

        private void OnMouseExit()
        {
            _placeHolder.SetActive(false);
        }

        private void OnMouseDown()
        {
            OnSlotClicked?.Invoke(Segment.Position);
        }
    }
}