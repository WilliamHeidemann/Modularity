using System.Collections.Generic;
using System.Linq;
using Runtime.Components.Segments;
using Runtime.Components.Utility;
using UnityEngine;
using UtilityToolkit.Runtime;

namespace Runtime.Scriptable_Objects
{
    [CreateAssetMenu]
    public class PlaceHolderBuilder : ScriptableObject
    {
        [SerializeField] private Selection _selection;
        [SerializeField] private Structure _structure;
        [SerializeField] private QuestFactory _questFactory;
        private Option<Segment> _placeHolder;
        private List<Quaternion> _rotations;
        private int _index = 0;
        [SerializeField] private Material _transparentValidMat;
        [SerializeField] private Material _transparentInvalidMat;

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

            var material = validRotations.Any() ? _transparentValidMat : _transparentInvalidMat;
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

        public void TearDown()
        {
            if (_placeHolder.IsSome(out var segment))
            {
                Destroy(segment.gameObject);
            }

            _placeHolder = Option<Segment>.None;
        }

        public void Hide()
        {
            if (_placeHolder.IsSome(out var segment))
            {
                segment.gameObject.SetActive(false);
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

            Debug.Log($"Index first: {_index}");
            _index += 1;
            _index %= _rotations.Count;
            Debug.Log($"Index last: {_index}");
            Debug.Log($"Rotation first: {segment.transform.rotation}");
            segment.transform.rotation = _rotations[_index];
            Debug.Log($"Rotation last: {segment.transform.rotation}");
            _questFactory.SegmentRotated();
        }

        private IEnumerable<SegmentData> ValidRotations(Vector3Int position, StaticSegmentData staticSegmentData,
            bool disregardValidity = false)
        {
            List<HashSet<Vector3Int>> seen = new();

            return AllRotations()
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

        private static IEnumerable<Quaternion> AllRotations()
        {
            // Define the 6 primary orientations with each axis facing "up" (aligned with +Z)
            var primaryRotations = new[]
            {
                Quaternion.identity, // +Z
                Quaternion.Euler(90, 0, 0), // +X
                Quaternion.Euler(0, 90, 0), // +Y
                Quaternion.Euler(0, -90, 0), // -Y
                Quaternion.Euler(-90, 0, 0), // -X
                Quaternion.Euler(180, 0, 0) // -Z
            };

            // For each primary orientation, add 4 rotations around the current +Z axis
            foreach (var baseRotation in primaryRotations)
            {
                yield return baseRotation * Quaternion.Euler(0, 0, 0); // 0° around Z
                yield return baseRotation * Quaternion.Euler(0, 0, 90); // 90° around Z
                yield return baseRotation * Quaternion.Euler(0, 0, 180); // 180° around Z
                yield return baseRotation * Quaternion.Euler(0, 0, 270); // 270° around Z
            }
        }
    }
}