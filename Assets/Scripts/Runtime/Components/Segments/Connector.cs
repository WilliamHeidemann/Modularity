using System;
using System.Collections.Generic;
using System.Linq;
using Runtime.Models;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Runtime.Components.Segments
{
    public class Connector : MonoSegment
    {
        [SerializeField] private bool _connectsUp;
        [SerializeField] private bool _connectsDown;
        [SerializeField] private bool _connectsForward;
        [SerializeField] private bool _connectsBack;
        [SerializeField] private bool _connectsRight;
        [SerializeField] private bool _connectsLeft;

        [SerializeField] private GameObject _upPart;
        [SerializeField] private GameObject _downPart;
        [SerializeField] private GameObject _rightPart;
        [SerializeField] private GameObject _leftPart;
        [SerializeField] private GameObject _frontPart;
        [SerializeField] private GameObject _backPart;
        
        public static event Action<IEnumerable<Position>> OnSpawnSlots; 
        
        private void Start()
        {
            SetConnections();
            EnableParts();
            SpawnConnections();
        }

        private void SpawnConnections()
        {
            var slots = PlaceholderPositions().Select(p => p + Segment.Position);
            OnSpawnSlots?.Invoke(slots);
        }      
        
        private void SetConnections()
        {
            _connectsUp = Random.value < 0.5f;
            _connectsDown = Random.value < 0.5f;
            _connectsForward = Random.value < 0.5f;
            _connectsBack = Random.value < 0.5f;
            _connectsRight = Random.value < 0.5f;
            _connectsLeft = Random.value < 0.5f;
        }

        private void EnableParts()
        {
            _upPart.SetActive(_connectsUp);
            _downPart.SetActive(_connectsDown);
            _rightPart.SetActive(_connectsRight);
            _leftPart.SetActive(_connectsLeft);
            _frontPart.SetActive(_connectsForward);
            _backPart.SetActive(_connectsBack);
        }
        
        private IEnumerable<Position> PlaceholderPositions()
        {
            if (_connectsUp) yield return Position.Up;
            if (_connectsDown) yield return Position.Down;
            if (_connectsForward) yield return Position.Forward;
            if (_connectsBack) yield return Position.Back;
            if (_connectsRight) yield return Position.Right;
            if (_connectsLeft) yield return Position.Left;
        }
    }
}
