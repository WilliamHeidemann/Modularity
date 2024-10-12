using System;
using Runtime.Models;
using UnityEngine;

namespace Runtime.Components
{
    public class ConnectionSlot : MonoSegment
    {
        [SerializeField] private GameObject _placeHolder;

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
            SegmentFactory.Instance.PlaceSegment(new Segment(Segment.Position, Rotation.Forward, Kind.ConnectorBox));
        }
    }
}