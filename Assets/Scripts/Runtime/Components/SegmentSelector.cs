using Runtime.Models;
using UnityEngine;
using UnityUtils;

namespace Runtime.Components
{
    public class SegmentSelector : MonoBehaviour
    {
        public void SelectConnectorBox()
        {
            var connectorBox = new Segment(new Position(), Kind.ConnectorBox, SegmentStats.ConnectorBox);
            SegmentFactory.Instance.Select(connectorBox);
        }
        
        public void SelectCogs()
        {
            var cogs = new Segment(new Position(), Kind.Cogs, SegmentStats.Cogs);
            SegmentFactory.Instance.Select(cogs);
        }
        
        public void SelectPipes()
        {
            var pipes = new Segment(new Position(), Kind.Pipes, SegmentStats.Pipes);
            SegmentFactory.Instance.Select(pipes);
        }
    }
}