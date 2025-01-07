using System;
using System.Collections.Generic;
using Runtime.Components.Segments;
using UnityEngine;
using UtilityToolkit.Runtime;

namespace Runtime.DataLayer
{
    public static class SegmentDataExtensions
    {
        public static bool CanConnect(this SegmentData segmentData1, SegmentData segmentData2)
        {
            bool connectsDirectionally = CanConnectDirectionally(
                segmentData1, segmentData2,
                out (Vector3Int, ConnectionType) connection1,
                out (Vector3Int, ConnectionType) connection2);

            if (!connectsDirectionally)
            {
                return false;
            }

            return connection1.Item2 == connection2.Item2;
        }

        public static bool CanConnectDirectionally(this SegmentData segmentData1, SegmentData segmentData2,
            out (Vector3Int, ConnectionType) connection1, out (Vector3Int, ConnectionType) connection2)
        {
            var connection1Option = segmentData1.GetConnectionPointsPlus().FirstOption(point =>
                point.position == segmentData2.Position);
            var connection2Option = segmentData2.GetConnectionPointsPlus().FirstOption(point =>
                point.position == segmentData1.Position);

            bool connectsOut = connection1Option.IsSome(out (Vector3Int position, ConnectionType type) c1);
            bool connectsIn = connection2Option.IsSome(out (Vector3Int position, ConnectionType type) c2);
            connection1 = c1;
            connection2 = c2;
            return connectsOut && connectsIn;
        }

        public static string ToJson(this List<SegmentData> segmentDataList)
        {
            return JsonUtility.ToJson(new SegmentDataListWrapper(segmentDataList));
        }

        public static string ToJson(this IEnumerable<SegmentData> segmentDataList)
        {
            return JsonUtility.ToJson(new SegmentDataListWrapper(segmentDataList));
        }
        
        public static Structure ToStructure(this string json)
        {
            try
            {
                var segmentDataListWrapper = JsonUtility.FromJson<SegmentDataListWrapper>(json);
                var structure = ScriptableObject.CreateInstance<Structure>();
                foreach (var segmentData in segmentDataListWrapper.SegmentDataList)
                {
                    structure.AddSegment(segmentData);
                }
                return structure;
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                return null;
            }
        }
    }

    public class SegmentDataListWrapper
    {
        public List<SegmentData> SegmentDataList;

        public SegmentDataListWrapper(List<SegmentData> segmentDataList)
        {
            SegmentDataList = segmentDataList;
        }

        public SegmentDataListWrapper(IEnumerable<SegmentData> segmentData)
        {
            SegmentDataList = new List<SegmentData>(segmentData);
        }
    }
}