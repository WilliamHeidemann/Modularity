using System;
using Runtime.Models;
using UnityEngine;

namespace Runtime.Components.Segments
{
    public class Slot : MonoBehaviour
    {
        [SerializeField] private GameObject _placeHolder;
        public Position Position;
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
            OnSlotClicked?.Invoke(Position);
        }
    }
}