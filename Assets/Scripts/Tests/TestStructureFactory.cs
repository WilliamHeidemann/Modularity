using System;
using Runtime.DataLayer;
using UnityEngine;

namespace Tests
{
    internal class TestStructureFactory
    {
        /*
         *  + . .
         *  + . .
         *  + . .
         */
        public static Structure CreateStructure()
        {
            var structure = ScriptableObject.CreateInstance<Structure>();
            var segmentData00 = new SegmentData
            {
                StaticSegmentData = GetStaticSegment(Pipe.Cross, ConnectionType.Blood),
                Position = new Vector3Int(0, 0, 0),
            };
            var segmentData01 = new SegmentData
            {
                StaticSegmentData = GetStaticSegment(Pipe.Cross, ConnectionType.Blood),
                Position = new Vector3Int(0, 1, 0),
            };
            var segmentData02 = new SegmentData
            {
                StaticSegmentData = GetStaticSegment(Pipe.Cross, ConnectionType.Blood),
                Position = new Vector3Int(0, 2, 0),
            };

            structure.AddSegment(segmentData00);
            structure.AddSegment(segmentData01);
            structure.AddSegment(segmentData02);
            return structure;
        }
        
        public static StaticSegmentData GetStaticSegment(Pipe pipe, ConnectionType type)
        {
            var staticSegmentData = ScriptableObject.CreateInstance<StaticSegmentData>();
            staticSegmentData.IsBlood = true;
            staticSegmentData.ConnectionPoints = GetConnectionPoints(pipe, type);
            return staticSegmentData;
        }

        private static ConnectionPoints GetConnectionPoints(Pipe pipe, ConnectionType type)
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