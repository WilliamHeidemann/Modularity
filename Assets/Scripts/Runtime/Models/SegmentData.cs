using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Runtime.Models
{
    [Serializable]
    public struct SegmentData
    {
        [SerializeField] private int _blood;
        [SerializeField] private int _energy;
        [SerializeField] private int _mechanical;

        private SegmentData(int blood, int energy, int mechanical)
        {
            _blood = blood;
            _energy = energy;
            _mechanical = mechanical;
        }

        public int Blood => _blood;
        public int Energy => _energy;
        public int Mechanical => _mechanical;

        public static SegmentData Default(Model model)
        {
            return model switch
            {
                Model.ConnectorBox => ConnectorBox,
                Model.ConnectionSlot => None,
                Model.Cogs => Cogs,
                Model.Pipes => Pipes,
                Model.Heart => Heart,
                Model.Eyes => Eyes,
                Model.Tentacle => Tentacle,
                Model.Wings => Wings,
                _ => throw new ArgumentOutOfRangeException(nameof(model), model, null)
            };
        }

        public SegmentData WithVariation()
        {
            _blood += Random.Range(-1, 2);
            _energy += Random.Range(-1, 2);
            _mechanical += Random.Range(-1, 2);
            return this;
        }
        
        public static readonly SegmentData None = new(blood: 0, energy: 0, mechanical: 0);
        private static readonly SegmentData ConnectorBox = new(blood: 0, energy: 0, mechanical: -4);
        private static readonly SegmentData Cogs = new(blood: 0, energy: 1, mechanical: 1);
        private static readonly SegmentData Pipes = new(blood: 0, energy: 0, mechanical: 3);
        private static readonly SegmentData Heart = new(blood: 5, energy: -3, mechanical: 0);
        private static readonly SegmentData Eyes = new(blood: 1, energy: 2, mechanical: 0);
        private static readonly SegmentData Tentacle = new(blood: 4, energy: -3, mechanical: 0);
        private static readonly SegmentData Wings = new(blood: -6, energy: 9, mechanical: 0);
    }
}