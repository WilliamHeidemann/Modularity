using System;
using UnityEngine;

namespace Runtime.Models
{
    [System.Serializable]
    public struct SegmentStats
    {
        [SerializeField] private int _blood;
        [SerializeField] private int _energy;
        [SerializeField] private int _mechanical;

        private SegmentStats(int blood, int energy, int mechanical)
        {
            _blood = blood;
            _energy = energy;
            _mechanical = mechanical;
        }

        public int Blood => _blood;
        public int Energy => _energy;
        public int Mechanical => _mechanical;

        public static SegmentStats Default(Kind kind)
        {
            return kind switch
            {
                Kind.ConnectorBox => ConnectorBox,
                Kind.ConnectionSlot => None,
                Kind.Cogs => Cogs,
                Kind.Pipes => Pipes,
                Kind.Heart => Heart,
                Kind.Eyes => Eyes,
                Kind.Tentacle => Tentacle,
                Kind.Wings => Wings,
                _ => throw new ArgumentOutOfRangeException(nameof(kind), kind, null)
            };
        }
        
        public static readonly SegmentStats None = new(blood: 0, energy: 0, mechanical: 0);
        public static readonly SegmentStats ConnectorBox = new(blood: 0, energy: 0, mechanical: -4);
        public static readonly SegmentStats Cogs = new(blood: 0, energy: 1, mechanical: 1);
        public static readonly SegmentStats Pipes = new(blood: 0, energy: 0, mechanical: 3);
        public static readonly SegmentStats Heart = new(blood: 5, energy: -3, mechanical: 0);
        public static readonly SegmentStats Eyes = new(blood: 1, energy: 2, mechanical: 0);
        public static readonly SegmentStats Tentacle = new(blood: 4, energy: -3, mechanical: 0);
        public static readonly SegmentStats Wings = new(blood: -6, energy: 9, mechanical: 0);
    }
}