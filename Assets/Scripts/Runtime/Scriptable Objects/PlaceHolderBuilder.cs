using System.Collections.Generic;
using System.Linq;
using Runtime.Components.Segments;
using UnityEngine;
using UtilityToolkit.Runtime;

namespace Runtime.Scriptable_Objects
{
    [CreateAssetMenu]
    public class PlaceHolderBuilder : ScriptableObject
    {
        [SerializeField] private Selection _selection;
        [SerializeField] private Structure _structure;
        private Option<Segment> _placeHolder;

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

            if (_placeHolder.IsSome(out var placeHolder) &&
                placeHolder.StaticSegmentData == selectedSegment.StaticSegmentData)
            {
                placeHolder.gameObject.SetActive(true);
                placeHolder.transform.position = position;
            }
            else
            {
                var newPlaceHolder = Instantiate(selectedSegment, position, Quaternion.identity);
                newPlaceHolder.GetComponent<BoxCollider>().enabled = false;
                _placeHolder = Option<Segment>.Some(newPlaceHolder);
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

        public void RotateOnY() => Rotate(Vector3.up);
        public void RotateOnX() => Rotate(Vector3.right);

        private void Rotate(Vector3 axis)
        {
            if (!_placeHolder.IsSome(out var segment))
            {
                return;
            }

            segment.transform.Rotate(axis, 90, Space.World);
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

        private Quaternion[] _validRotations;

        public void SetValidRotations(Vector3Int position, StaticSegmentData staticSegmentData)
        {
            HashSet<IEnumerable<Vector3Int>> seen = new();
            
            _validRotations = AllRotations()
                .Select(rotation => new SegmentData
                {
                    Position = position,
                    Rotation = rotation,
                    StaticSegmentData = staticSegmentData
                })
                .Where(segmentData => _structure.ConnectsToSomething(segmentData))
                .Where(segmentData => seen.Add(segmentData.GetConnectionPoints()))
                .Select(segmentData => segmentData.Rotation).ToArray();
        }
    }
}