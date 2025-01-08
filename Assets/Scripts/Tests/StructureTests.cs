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
    }
}