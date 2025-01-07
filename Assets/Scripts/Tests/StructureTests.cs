using System;
using System.Collections;
using NUnit.Framework;
using NUnit.Framework.Internal;
using Runtime.DataLayer;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;
using Object = System.Object;

namespace Tests
{
    public class StructureTests
    {
        private Structure _structure;

        [SetUp]
        public void Setup()
        {
            _structure = CreateStructure();
        }

        [TearDown]
        public void TearDown()
        {
            
        }
        
        [Test]
        public void PositionIsOpen()
        {
            // Arrange
            Structure structure = CreateStructure();
            var openPosition = new Vector3Int(0, 3, 0);
            var takenPosition = new Vector3Int(0, 0, 0);

            // Act
            bool isOpenPosition1 = structure.IsOpenPosition(openPosition);
            bool isOpenPosition2 = structure.IsOpenPosition(takenPosition);
            
            // Assert
            Assert.IsTrue(isOpenPosition1);
            Assert.IsFalse(isOpenPosition2);
        }

        [Test]
        public void ConnectsEverywhere()
        {
            // Arrange
            var segmentData10 = new SegmentData
            {
                StaticSegmentData = GetBloodCross(),
                Position = new Vector3Int(1, 0, 0),
            };
            var segmentData12 = new SegmentData
            {
                StaticSegmentData = GetBloodCross(),
                Position = new Vector3Int(1, 2, 0),
            };
            var segmentData21 = new SegmentData
            {
                StaticSegmentData = GetBloodCross(),
                Position = new Vector3Int(2, 1, 0),
            };
            _structure.AddSegment(segmentData10);
            _structure.AddSegment(segmentData12);
            _structure.AddSegment(segmentData21);
            
            var centerPiece = new SegmentData
            {
                StaticSegmentData = GetBloodCross(),
                Position = new Vector3Int(1, 1, 0),
            };
            
            var edgePiece = new SegmentData
            {
                StaticSegmentData = GetBloodCross(),
                Position = new Vector3Int(2, 2, 0),
            };
            
            // Act
            bool connectsEverywhere1 = _structure.ConnectsEverywhere(centerPiece);
            bool connectsEverywhere2 = _structure.ConnectsEverywhere(edgePiece);
            
            // Assert
            Assert.IsTrue(connectsEverywhere1);
            Assert.IsFalse(connectsEverywhere2);
        }
        
        [Test]
        public void ConnectsToAtLeastOneNeighbor()
        {
            // Arrange
            var segmentData03 = new SegmentData
            {
                StaticSegmentData = GetBloodCross(),
                Position = new Vector3Int(0, 3, 0),
            };
            var segmentData20 = new SegmentData
            {
                StaticSegmentData = GetBloodCross(),
                Position = new Vector3Int(2, 0, 0),
            };
            
            // Act
            bool connectsToAtLeastOneNeighbor1 = _structure.ConnectsToAtLeastOneNeighbor(segmentData03);
            bool connectsToAtLeastOneNeighbor2 = _structure.ConnectsToAtLeastOneNeighbor(segmentData20);
            
            // Assert
            Assert.IsTrue(connectsToAtLeastOneNeighbor1);
            Assert.IsFalse(connectsToAtLeastOneNeighbor2);
        }

        public Structure CreateStructure()
        {
            var structure = ScriptableObject.CreateInstance<Structure>();
            var segmentData00 = new SegmentData
            {
                StaticSegmentData = GetBloodCross(),
                Position = new Vector3Int(0, 0, 0),
            };
            var segmentData01 = new SegmentData
            {
                StaticSegmentData = GetBloodCross(),
                Position = new Vector3Int(0, 1, 0),
            };
            var segmentData02 = new SegmentData
            {
                StaticSegmentData = GetBloodCross(),
                Position = new Vector3Int(0, 2, 0),
            };

            structure.AddSegment(segmentData00);
            structure.AddSegment(segmentData01);
            structure.AddSegment(segmentData02);
            return structure;
        }

        public StaticSegmentData GetBloodCross()
        {
            var staticSegmentData = ScriptableObject.CreateInstance<StaticSegmentData>();
            staticSegmentData.IsBlood = true;
            staticSegmentData.ConnectionPoints = GetConnectionPoints(Pipe.Cross, ConnectionType.Blood);
            return staticSegmentData;
        }

        public ConnectionPoints GetConnectionPoints(Pipe pipe, ConnectionType type)
        {
            return pipe switch
            {
                Pipe.Cross => new ConnectionPoints
                {
                    Up = type,
                    Right = type,
                    Down = type,
                    Left = type
                },
                Pipe.Elbow => throw new ArgumentOutOfRangeException(nameof(pipe), pipe, null),
                Pipe.ElbowTee => throw new ArgumentOutOfRangeException(nameof(pipe), pipe, null),
                Pipe.QuadTee => throw new ArgumentOutOfRangeException(nameof(pipe), pipe, null),
                Pipe.Tee => throw new ArgumentOutOfRangeException(nameof(pipe), pipe, null),
                Pipe.Tree => throw new ArgumentOutOfRangeException(nameof(pipe), pipe, null),
                Pipe.TripleTee => throw new ArgumentOutOfRangeException(nameof(pipe), pipe, null),
                Pipe.Union => new ConnectionPoints
                {
                    Up = type,
                    Down = type,
                },
                _ => throw new ArgumentOutOfRangeException(nameof(pipe), pipe, null)
            };
        }

        public enum Pipe
        {
            Cross,
            Elbow,
            ElbowTee,
            QuadTee,
            Tee,
            Tree,
            TripleTee,
            Union
        }
    }
}