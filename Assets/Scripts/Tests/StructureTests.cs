using System;
using System.Collections;
using System.Linq;
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
            _structure = TestStructureFactory.CreateStructure();
        }

        [TearDown]
        public void TearDown()
        {
        }

        [Test]
        public void PositionIsOpen()
        {
            // Arrange
            var openPosition = new Vector3Int(0, 3, 0);
            var takenPosition = new Vector3Int(0, 0, 0);

            // Act
            bool isOpenPosition1 = _structure.IsOpenPosition(openPosition);
            bool isOpenPosition2 = _structure.IsOpenPosition(takenPosition);

            // Assert
            Assert.IsTrue(isOpenPosition1);
            Assert.IsFalse(isOpenPosition2);
        }

        [Test]
        public void ConnectsToAllNeighbors()
        {
            // Arrange
            var segmentData10 = new SegmentData
            {
                StaticSegmentData = TestStructureFactory.GetStaticSegment(TestStructureFactory.Pipe.Cross, ConnectionType.Blood),
                Position = new Vector3Int(1, 0, 0),
            };
            var segmentData12 = new SegmentData
            {
                StaticSegmentData = TestStructureFactory.GetStaticSegment(TestStructureFactory.Pipe.Cross, ConnectionType.Blood),
                Position = new Vector3Int(1, 2, 0),
            };
            var segmentData21 = new SegmentData
            {
                StaticSegmentData = TestStructureFactory.GetStaticSegment(TestStructureFactory.Pipe.Cross, ConnectionType.Blood),
                Position = new Vector3Int(2, 1, 0),
            };
            _structure.AddSegment(segmentData10);
            _structure.AddSegment(segmentData12);
            _structure.AddSegment(segmentData21);

            /*
             *  + + .
             *  + . +
             *  + + .
             */

            var centerCross = new SegmentData
            {
                StaticSegmentData = TestStructureFactory.GetStaticSegment(TestStructureFactory.Pipe.Cross, ConnectionType.Blood),
                Position = new Vector3Int(1, 1, 0),
            };

            /*
             *  + + .
             *  + + +
             *  + + .
             */

            var cornerCross = new SegmentData
            {
                StaticSegmentData = TestStructureFactory.GetStaticSegment(TestStructureFactory.Pipe.Cross, ConnectionType.Blood),
                Position = new Vector3Int(2, 2, 0),
            };

            /*
             *  + + +
             *  + . +
             *  + + .
             */

            var cornerUnion = new SegmentData
            {
                StaticSegmentData = TestStructureFactory.GetStaticSegment(TestStructureFactory.Pipe.Union, ConnectionType.Blood),
                Position = new Vector3Int(2, 0, 0),
            };

            /*
             *  + + .
             *  + . +
             *  + + |
             */

            // Act
            bool connectsEverywhere1 = _structure.ConnectsToAllNeighbors(centerCross);
            bool connectsEverywhere2 = _structure.ConnectsToAllNeighbors(cornerCross);
            bool connectsEverywhere3 = _structure.ConnectsToAllNeighbors(cornerUnion);

            // Assert
            Assert.IsTrue(connectsEverywhere1);
            Assert.IsTrue(connectsEverywhere2);
            Assert.IsFalse(connectsEverywhere3);
        }

        [Test]
        public void ConnectsToAtLeastOneNeighbor()
        {
            // Arrange
            var segmentData03 = new SegmentData
            {
                StaticSegmentData = TestStructureFactory.GetStaticSegment(TestStructureFactory.Pipe.Cross, ConnectionType.Blood),
                Position = new Vector3Int(0, 3, 0),
            };
            var segmentData20 = new SegmentData
            {
                StaticSegmentData = TestStructureFactory.GetStaticSegment(TestStructureFactory.Pipe.Cross, ConnectionType.Blood),
                Position = new Vector3Int(2, 0, 0),
            };

            // Act
            bool connectsToAtLeastOneNeighbor1 = _structure.ConnectsToAtLeastOneNeighbor(segmentData03);
            bool connectsToAtLeastOneNeighbor2 = _structure.ConnectsToAtLeastOneNeighbor(segmentData20);

            // Assert
            Assert.IsTrue(connectsToAtLeastOneNeighbor1);
            Assert.IsFalse(connectsToAtLeastOneNeighbor2);
        }

        [Test]
        public void GetPointedToSegments()
        {
            var union11 = new SegmentData
            {
                StaticSegmentData = TestStructureFactory.GetStaticSegment(TestStructureFactory.Pipe.Union, ConnectionType.Blood),
                Position = new Vector3Int(1, 1, 0),
            };

            var union20 = new SegmentData
            {
                StaticSegmentData = TestStructureFactory.GetStaticSegment(TestStructureFactory.Pipe.Union, ConnectionType.Blood),
                Position = new Vector3Int(2, 0, 0),
            };

            _structure.AddSegment(union11);
            _structure.AddSegment(union20);

            var cross = new SegmentData
            {
                StaticSegmentData = TestStructureFactory.GetStaticSegment(TestStructureFactory.Pipe.Cross, ConnectionType.Blood),
                Position = new Vector3Int(1, 0, 0),
            };
            
            /*
             *  + .
             *  + |
             *  +(+)|
             */

            var union = new SegmentData
            {
                StaticSegmentData = TestStructureFactory.GetStaticSegment(TestStructureFactory.Pipe.Union, ConnectionType.Blood),
                Position = new Vector3Int(2, 1, 0),
            };
            
            /*
             *  + .
             *  + |(|)
             *  + . |
             */


            // Act
            var crossPointedToSegments =
                _structure.GetPointedToSegments(cross).Select(segment => segment.Position).ToList();

            var unionPointedToSegments =
                _structure.GetPointedToSegments(union).Select(segment => segment.Position).ToList();
            
            // Assert
            Assert.IsTrue(crossPointedToSegments.Contains(new Vector3Int(0, 0, 0)));
            Assert.IsTrue(crossPointedToSegments.Contains(new Vector3Int(1, 1, 0)));
            Assert.IsTrue(crossPointedToSegments.Contains(new Vector3Int(2, 0, 0)));
            Assert.IsFalse(crossPointedToSegments.Contains(new Vector3Int(1, 0, 0)));
            Assert.IsFalse(crossPointedToSegments.Contains(new Vector3Int(1, 2, 0)));
            Assert.IsFalse(crossPointedToSegments.Contains(new Vector3Int(1, -1, 0)));
            
            Assert.IsTrue(unionPointedToSegments.Contains(new Vector3Int(2, 0, 0)));
            Assert.IsFalse(unionPointedToSegments.Contains(new Vector3Int(1, 1, 0)));
        }

        [Test]
        public void GetPointedFromSegments()
        {
            var union11 = new SegmentData
            {
                StaticSegmentData = TestStructureFactory.GetStaticSegment(TestStructureFactory.Pipe.Union, ConnectionType.Blood),
                Position = new Vector3Int(1, 1, 0),
            };

            var union20 = new SegmentData
            {
                StaticSegmentData = TestStructureFactory.GetStaticSegment(TestStructureFactory.Pipe.Union, ConnectionType.Blood),
                Position = new Vector3Int(2, 0, 0),
            };

            _structure.AddSegment(union11);
            _structure.AddSegment(union20);

            var cross = new SegmentData
            {
                StaticSegmentData = TestStructureFactory.GetStaticSegment(TestStructureFactory.Pipe.Cross, ConnectionType.Blood),
                Position = new Vector3Int(1, 0, 0),
            };

            /*
             *  + .
             *  + |
             *  +(+)|
             */


            // Act
            var pointedFromSegments =
                _structure.GetPointedFromSegments(cross).Select(segment => segment.Position).ToList();

            // Assert
            Assert.IsTrue(pointedFromSegments.Contains(new Vector3Int(0, 0, 0)));
            Assert.IsTrue(pointedFromSegments.Contains(new Vector3Int(1, 1, 0)));
            Assert.IsFalse(pointedFromSegments.Contains(new Vector3Int(2, 0, 0)));
            Assert.IsFalse(pointedFromSegments.Contains(new Vector3Int(1, 0, 0)));
            Assert.IsFalse(pointedFromSegments.Contains(new Vector3Int(0, 2, 0)));
            Assert.IsFalse(pointedFromSegments.Contains(new Vector3Int(1, -1, 0)));
        }

        [Test]
        public void GetValidConnections()
        {
            var union11 = new SegmentData
            {
                StaticSegmentData = TestStructureFactory.GetStaticSegment(TestStructureFactory.Pipe.Union, ConnectionType.Blood),
                Position = new Vector3Int(1, 1, 0),
            };

            var union20 = new SegmentData
            {
                StaticSegmentData = TestStructureFactory.GetStaticSegment(TestStructureFactory.Pipe.Union, ConnectionType.Blood),
                Position = new Vector3Int(2, 0, 0),
            };

            _structure.AddSegment(union11);
            _structure.AddSegment(union20);

            var cross = new SegmentData
            {
                StaticSegmentData = TestStructureFactory.GetStaticSegment(TestStructureFactory.Pipe.Cross, ConnectionType.Blood),
                Position = new Vector3Int(1, 0, 0),
            };
            
            /*
             *  + .
             *  + |
             *  +(+)|
             */
            
            var union = new SegmentData
            {
                StaticSegmentData = TestStructureFactory.GetStaticSegment(TestStructureFactory.Pipe.Union, ConnectionType.Blood),
                Position = new Vector3Int(2, 1, 0),
            };
            
            /*
             *  + .
             *  + |(|)
             *  + . |
             */

            // Act
            var pointedFromSegments =
                _structure.GetValidConnections(cross).Select(segment => segment.Position).ToList();
            
            var unionPointedFromSegments =
                _structure.GetValidConnections(union).Select(segment => segment.Position).ToList();
            
            // Assert
            Assert.IsTrue(pointedFromSegments.Contains(new Vector3Int(0, 0, 0)));
            Assert.IsTrue(pointedFromSegments.Contains(new Vector3Int(1, 1, 0)));
            Assert.IsFalse(pointedFromSegments.Contains(new Vector3Int(2, 0, 0)));
            Assert.IsFalse(pointedFromSegments.Contains(new Vector3Int(1, 0, 0)));
            Assert.IsFalse(pointedFromSegments.Contains(new Vector3Int(0, 2, 0)));
            Assert.IsFalse(pointedFromSegments.Contains(new Vector3Int(1, -1, 0)));
            
            Assert.IsTrue(unionPointedFromSegments.Contains(new Vector3Int(2, 0, 0)));
            Assert.IsFalse(unionPointedFromSegments.Contains(new Vector3Int(1, 1, 0)));
        }

        [Test]
        public void GetPointedFromConnectionTypes()
        {
            var union11 = new SegmentData
            {
                StaticSegmentData = TestStructureFactory.GetStaticSegment(TestStructureFactory.Pipe.Union, ConnectionType.Steam),
                Position = new Vector3Int(1, 1, 0),
            };

            var union20 = new SegmentData
            {
                StaticSegmentData = TestStructureFactory.GetStaticSegment(TestStructureFactory.Pipe.Union, ConnectionType.Steam),
                Position = new Vector3Int(2, 0, 0),
            };

            _structure.AddSegment(union11);
            _structure.AddSegment(union20);

            var cross = new SegmentData
            {
                StaticSegmentData = TestStructureFactory.GetStaticSegment(TestStructureFactory.Pipe.Cross, ConnectionType.Blood),
                Position = new Vector3Int(1, 0, 0),
            };
            
            /*
             *  + .
             *  + |
             *  +(+)|
             */
            
            var union = new SegmentData
            {
                StaticSegmentData = TestStructureFactory.GetStaticSegment(TestStructureFactory.Pipe.Union, ConnectionType.Steam),
                Position = new Vector3Int(2, 1, 0),
            };
            
            /*
             *  + .
             *  + |(|)
             *  + . |
             */
            
            // Act
            var pointedFromConnectionTypes =
                _structure.GetPointedFromConnectionTypes(cross).ToList();

            var unionPointedFromConnectionTypes =
                _structure.GetPointedFromConnectionTypes(union).ToList();
            
            // Assert
            Assert.AreEqual(1, pointedFromConnectionTypes.Count(type => type == ConnectionType.Blood));
            Assert.AreEqual(1, pointedFromConnectionTypes.Count(type => type == ConnectionType.Steam));
            
            Assert.AreEqual(0, unionPointedFromConnectionTypes.Count(type => type == ConnectionType.Blood));
            Assert.AreEqual(1, unionPointedFromConnectionTypes.Count(type => type == ConnectionType.Steam));
        }

        [Test]
        public void IsValidPlacement()
        {
            // Arrange
            var union10 = new SegmentData
            {
                StaticSegmentData = TestStructureFactory.GetStaticSegment(TestStructureFactory.Pipe.Union, ConnectionType.Blood),
                Position = new Vector3Int(1, 0, 0),
            };
            
            var unionBlood03 = new SegmentData
            {
                StaticSegmentData = TestStructureFactory.GetStaticSegment(TestStructureFactory.Pipe.Union, ConnectionType.Blood),
                Position = new Vector3Int(0, 3, 0),
            };
            
            var unionSteam03 = new SegmentData
            {
                StaticSegmentData = TestStructureFactory.GetStaticSegment(TestStructureFactory.Pipe.Union, ConnectionType.Steam),
                Position = new Vector3Int(0, 3, 0),
            };

            var union00 = new SegmentData
            {
                StaticSegmentData = TestStructureFactory.GetStaticSegment(TestStructureFactory.Pipe.Union, ConnectionType.Blood),
                Position = new Vector3Int(0, 0, 0),
            };

            // Act
            bool isValidPlacement10 = _structure.IsValidPlacement(union10);
            bool isValidPlacement03Blood = _structure.IsValidPlacement(unionBlood03);
            bool isValidPlacement03Steam = _structure.IsValidPlacement(unionSteam03);
            bool isValidPlacement00 = _structure.IsValidPlacement(union00);
            
            // Assert
            Assert.IsFalse(isValidPlacement10);
            Assert.IsTrue(isValidPlacement03Blood);
            Assert.IsFalse(isValidPlacement03Steam);
            Assert.IsFalse(isValidPlacement00);
        }

        [Test]
        public void GetNeighborsConnectingDirectionally()
        {
            // Arrange
            var union10 = new SegmentData
            {
                StaticSegmentData =
                    TestStructureFactory.GetStaticSegment(TestStructureFactory.Pipe.Union, ConnectionType.Blood),
                Position = new Vector3Int(1, 0, 0),
            };

            var union12 = new SegmentData
            {
                StaticSegmentData =
                    TestStructureFactory.GetStaticSegment(TestStructureFactory.Pipe.Union, ConnectionType.Blood),
                Position = new Vector3Int(1, 2, 0),
            };

            var union21 = new SegmentData
            {
                StaticSegmentData =
                    TestStructureFactory.GetStaticSegment(TestStructureFactory.Pipe.Union, ConnectionType.Blood),
                Position = new Vector3Int(2, 1, 0),
            };

            _structure.AddSegment(union10);
            _structure.AddSegment(union12);
            _structure.AddSegment(union21);

            var cross11 = new SegmentData
            {
                StaticSegmentData =
                    TestStructureFactory.GetStaticSegment(TestStructureFactory.Pipe.Cross, ConnectionType.Blood),
                Position = new Vector3Int(1, 1, 0),
            };

            /*
             *  + |
             *  + + |
             *  + |
             */

            // Act
            var neighborsConnectingDirectionally = _structure.GetNeighborsConnectingDirectionally(cross11)
                .Select(segment => segment.Position).ToList();

            // Assert
            Assert.IsTrue(neighborsConnectingDirectionally.Contains(new Vector3Int(0, 1, 0)));
            Assert.IsTrue(neighborsConnectingDirectionally.Contains(new Vector3Int(1, 0, 0)));
            Assert.IsTrue(neighborsConnectingDirectionally.Contains(new Vector3Int(1, 2, 0)));
            Assert.IsFalse(neighborsConnectingDirectionally.Contains(new Vector3Int(2, 1, 0)));
            Assert.IsFalse(neighborsConnectingDirectionally.Contains(new Vector3Int(0, 0, 0)));
            Assert.IsFalse(neighborsConnectingDirectionally.Contains(new Vector3Int(1, 1, 0)));
        }

        [Test]
        public void IsValidSourcePlacement()
        {
            // Act
            bool isValidSourcePosition1 = _structure.IsValidSourcePlacement(new Vector3Int(0, 0, 0));
            bool isValidSourcePosition2 = _structure.IsValidSourcePlacement(new Vector3Int(1, 0, 0));
            bool isValidSourcePosition3 = _structure.IsValidSourcePlacement(new Vector3Int(2, 0, 0));
            
            // Assert
            Assert.IsFalse(isValidSourcePosition1);
            Assert.IsFalse(isValidSourcePosition2);
            Assert.IsTrue(isValidSourcePosition3);
        }
    }
}