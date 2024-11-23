using System;
using UnityEngine;

namespace Runtime.Components.Utility
{
    public class FogController : MonoBehaviour
    {
        [SerializeField] private Transform _transform;
        [SerializeField] private AnimationCurve _fogDensityCurve;
        private void Update()
        {
            var distance = _transform.position.magnitude;
            var density = _fogDensityCurve.Evaluate(distance);
            RenderSettings.fogDensity = density;
        }
    }
}