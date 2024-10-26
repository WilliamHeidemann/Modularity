using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Runtime.Components.Utility;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Runtime.Components.Segments
{
    public class Segment : MonoBehaviour
    {
        public ConnectionPoints ConnectionPoints;

        [SerializeField] private GameObject _upPart;
        [SerializeField] private GameObject _downPart;
        [SerializeField] private GameObject _rightPart;
        [SerializeField] private GameObject _leftPart;
        [SerializeField] private GameObject _frontPart;
        [SerializeField] private GameObject _backPart;

        private void Start()
        {
            EnableParts();
        }

        private void EnableParts()
        {
            _upPart.SetActive(ConnectionPoints.Up);
            _downPart.SetActive(ConnectionPoints.Down);
            _rightPart.SetActive(ConnectionPoints.Right);
            _leftPart.SetActive(ConnectionPoints.Left);
            _frontPart.SetActive(ConnectionPoints.Forward);
            _backPart.SetActive(ConnectionPoints.Back);
        }

        public IEnumerable<Vector3Int> AdjacentPlaceholderPositions() =>
            ConnectionPoints
                .AsVector3Ints()
                .Select(position => transform.position.AsVector3Int() + position);
    }
}