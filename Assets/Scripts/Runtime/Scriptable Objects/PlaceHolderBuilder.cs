using System.Collections.Generic;
using System.Linq;
using Runtime.Components.Segments;
using UnityEngine;
using UnityUtils;
using UtilityToolkit.Runtime;

namespace Runtime.Scriptable_Objects
{
    [CreateAssetMenu]
    public class PlaceHolderBuilder : ScriptableObject
    {
        [SerializeField] private Selection _selection;
        [SerializeField] private Structure _structure;
        private Option<Segment> _placeHolder;
        private List<Quaternion> _validRotations;
        private int _index = 0;
        [SerializeField] private Material _transparentMatBlue;
        [SerializeField] private Material _transparentMatRed;

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
            
            SetValidRotations(position, selectedSegment.StaticSegmentData);

            if (!_validRotations.Any())
            {
                return;
            }

            if (_placeHolder.IsSome(out var placeHolder) &&
                placeHolder.StaticSegmentData == selectedSegment.StaticSegmentData)
            {
                placeHolder.gameObject.SetActive(true);
                placeHolder.transform.position = position;
            }
            else
            {
                placeHolder = Instantiate(selectedSegment, position, Quaternion.identity);
                placeHolder.GetComponent<BoxCollider>().enabled = false;
                Material transparentMat;
                if(placeHolder.StaticSegmentData.Steam)
                {
                    transparentMat = _transparentMatBlue;
                }
                else
                {
                    transparentMat = _transparentMatRed;
                }
                foreach (var meshRenderer in placeHolder.GetComponentsInChildren<MeshRenderer>())
                {
                    meshRenderer.material = transparentMat;
                }
                _placeHolder = Option<Segment>.Some(placeHolder);
            }

            if (!_validRotations.Contains(placeHolder.transform.rotation))
            {
                placeHolder.transform.rotation = _validRotations.First();
                _index = 0;
            }
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

            if (!_validRotations.Any())
            {
                return;
            }
            
            _index += 1;
            _index %= _validRotations.Count;
            segment.transform.rotation = _validRotations[_index];
        }

        private void SetValidRotations(Vector3Int position, StaticSegmentData staticSegmentData)
        {
            List<HashSet<Vector3Int>> seen = new();

            _validRotations = AllRotations()
                .Select(rotation => new SegmentData
                {
                    Position = position,
                    Rotation = rotation,
                    StaticSegmentData = staticSegmentData
                })
                .Where(segmentData => _structure.ConnectsToSomething(segmentData))
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
                .Select(segmentData => segmentData.Rotation).ToList();
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