using System;
using System.Collections.Generic;
using System.Linq;
using Runtime.Backend;
using Runtime.Components.Segments;
using Runtime.Components.Utility;
using Runtime.DataLayer;
using UnityEngine;
using UtilityToolkit.Runtime;

namespace Runtime.Scriptable_Objects
{
    [CreateAssetMenu]
    public class PlaceHolderBuilder : ScriptableObject
    {
        [SerializeField] private Selection _selection;
        [SerializeField] private Structure _structure;
        [SerializeField] private Currency _currency;
        [SerializeField] private QuestFactory _questFactory;
        private Option<Segment> _placeHolder;
        private List<Quaternion> _rotations;
        private int _index = 0;
        [SerializeField] private Material _transparentValidMat;
        [SerializeField] private Material _transparentInvalidMat;
        public event Action OnSegmentCanRotate;
        public event Action OnSegmentCannotRotate;

        public void Build(Vector3Int position)
        {
            if (!_structure.IsOpenPosition(position))
            {
                return;
            }

            if (!_selection.Prefab.IsSome(out var selectedSegment))
            {
                return;
            }

            var validSegmentData = ValidRotations(position, selectedSegment.StaticSegmentData).ToList();
            var validRotations = validSegmentData.Select(segmentData => segmentData.Rotation).ToList();
            var directionallyValidRotations = ValidRotations(position, selectedSegment.StaticSegmentData, true).Select(segmentData => segmentData.Rotation).ToList();
            _rotations = validRotations.Any() ? validRotations : directionallyValidRotations;

            if (_placeHolder.IsSome(out var placeHolder) &&
                placeHolder.StaticSegmentData == selectedSegment.StaticSegmentData)
            {
                placeHolder.gameObject.SetActive(true);
                placeHolder.transform.position = position;
            }
            else
            {
                TearDown();
                placeHolder = Instantiate(selectedSegment, position, _rotations.FirstOrDefault());
                _placeHolder = Option<Segment>.Some(placeHolder);
            }
            
            if (validRotations.Count > 1)
            {
                OnSegmentCanRotate?.Invoke();
            }

            var hasEnoughCurrency = _currency.HasAtLeast(_selection.PriceBlood, _selection.PriceSteam);
                
            var material = validRotations.Any() && hasEnoughCurrency
                ? _transparentValidMat 
                : _transparentInvalidMat;
            foreach (var meshRenderer in placeHolder.GetComponentsInChildren<MeshRenderer>())
            {
                var newMaterials = meshRenderer.sharedMaterials.Select(mat => material).ToArray();
                meshRenderer.sharedMaterials = newMaterials;
            }
            
            if (!_rotations.ContainsRotation(placeHolder.transform.rotation))
            {
                placeHolder.transform.rotation = _rotations.First();
                _index = 0;
            }
            else
            {
                _index = _rotations.IndexOfRotation(placeHolder.transform.rotation);
            }

            if (validSegmentData.Count == 0)
            {
                return;
            }
            
            var segmentData = new SegmentData
            {
                Position = position,
                Rotation = placeHolder.transform.rotation,
                StaticSegmentData = selectedSegment.StaticSegmentData
            };

            var connections = _structure.GetValidConnections(segmentData).Count();
            var maxConnections = _structure.GetValidConnections(validSegmentData.First()).Count();
            if (maxConnections > connections)
            {
                placeHolder.transform.rotation = _rotations.First();
                _index = 0;
            }
                
            SoundFXPlayer.Instance.Play(SoundFX.CardSelection);
        }

        public void Clear()
        {
            if (_placeHolder.IsSome(out var segment))
            {
                if (segment == null)
                {
                    _placeHolder = Option<Segment>.None;
                    return;
                }
                
                Destroy(segment.gameObject);
                OnSegmentCannotRotate?.Invoke();
            }
            
            _placeHolder = Option<Segment>.None;
        }

        public void TearDown()
        {
            if (_placeHolder.IsSome(out var segment))
            {
                Destroy(segment.gameObject);
                OnSegmentCannotRotate?.Invoke();
            }

            _placeHolder = Option<Segment>.None;
        }

        public void Hide()
        {
            if (_placeHolder.IsSome(out var segment))
            {
                segment.gameObject.SetActive(false);
                OnSegmentCannotRotate?.Invoke();
            }
        }

        public Quaternion PlaceholderRotation()
        {
            return _placeHolder.IsSome(out var segment) ? segment.transform.rotation : Quaternion.identity;
        }

        public void Rotate()
        {
            if (!_placeHolder.IsSome(out var segment))
            {
                return;
            }

            if (_rotations.Count <= 1)
            {
                return;
            }

            _index += 1;
            _index %= _rotations.Count;
            TweenAnimations.RotateTransform(segment.transform, _rotations[_index]);
            SoundFXPlayer.Instance.Play(SoundFX.RotationWhoosh);
            _questFactory.RotateSegment();
        }

        private IEnumerable<SegmentData> ValidRotations(Vector3Int position, StaticSegmentData staticSegmentData,
            bool disregardValidity = false)
        {
            List<HashSet<Vector3Int>> seen = new();

            return RotationUtility.AllRotations()
                .Select(rotation => new SegmentData
                {
                    Position = position,
                    Rotation = rotation,
                    StaticSegmentData = staticSegmentData
                })
                .Where(segmentData =>
                    disregardValidity
                        ? _structure.IsDirectionallyValidPlacement(segmentData)
                        : _structure.IsValidPlacement(segmentData))
                .Where(segmentData =>
                {
                    var points = segmentData.GetConnectionPoints().ToHashSet();
                    var unique = !seen.Any(other => other.SetEquals(points));
                    if (unique)
                    {
                        seen.Add(points);
                    }

                    return unique;
                })
                .OrderByDescending(segmentData => _structure.GetValidConnections(segmentData).Count());
            // .Select(segmentData => segmentData.Rotation);
        }
    }
}