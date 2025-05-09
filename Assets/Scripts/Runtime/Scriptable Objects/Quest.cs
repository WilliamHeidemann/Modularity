using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Runtime.DataLayer;
using TMPro;

namespace Runtime.Scriptable_Objects
{
    [Serializable]
    public class Quest
    {
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField]
        [TextArea(3, 10)]
        public string Description { get; protected set; }
        
        public Action OnComplete;
        [HideInInspector] public TextMeshProUGUI DescriptionText;

        protected Quest(string name, string description)
        {
            Name = name;
            Description = description;
        }

        public void Complete()
        {
            OnComplete?.Invoke();
            OnComplete = null;
        }

        public virtual Quest Build() => new(Name, Description);
    }

    [Serializable]
    public class Quest<T> : Quest
    {
        protected int Count;
        [SerializeField] protected int Target;

        protected Quest(string name, string description, int target) : base(name, description)
        {
            Target = target;
            Description = Regex.Replace(Description, @"/\d", $"/{target}");
        }

        public virtual Quest<T> Build(int target) => new(Name, Description, target);
        public override Quest Build() => new Quest<int>(Name, Description, Target);

        protected void UpdateDescription()
        {
            Description = Regex.Replace(Description, @"\d+/", $"{Count}/");
            if (Count >= Target)
            {
                return;
            }

            DescriptionText.text = Description;
        }

        public virtual void Progress(T t)
        {
            if (t is int amount)
            {
                Count += amount;
            }
            else
            {
                throw new ArgumentException($"Progression for type {typeof(T)} not implemented");
            }

            if (Count >= Target)
            {
                Complete();
            }

            UpdateDescription();
        }
    }

    [Serializable]
    public class SegmentQuest : Quest<SegmentData>
    {
        [SerializeField] private bool _countBlood;
        [SerializeField] private bool _countSteam;

        protected SegmentQuest(string name, string description, int target, bool countBlood, bool countSteam)
            : base(name, description, target)
        {
            _countBlood = countBlood;
            _countSteam = countSteam;
        }

        public override Quest<SegmentData> Build(int target) =>
            new SegmentQuest(Name, Description, target, _countBlood, _countSteam);

        public override void Progress(SegmentData segments)
        {
            if (_countBlood && segments.StaticSegmentData.IsBlood)
            {
                Count++;
            }

            else if (_countSteam && segments.StaticSegmentData.IsSteam)
            {
                Count++;
            }

            if (Count >= Target)
            {
                Complete();
            }

            UpdateDescription();
        }
    }

    [Serializable]
    public class HybridQuest : Quest<SegmentData>
    {
        protected HybridQuest(string name, string description, int target) : base(name, description, target)
        {
        }
        
        public override Quest<SegmentData> Build(int target) => new HybridQuest(Name, Description, target);
        
        public override void Progress(SegmentData segments)
        {
            if (segments.StaticSegmentData.IsBlood && segments.StaticSegmentData.IsSteam)
            {
                Count++;
            }

            if (Count >= Target)
            {
                Complete();
            }

            UpdateDescription();
        }
    }


    [Serializable]
    public class ReceiverQuest : Quest<IEnumerable<SegmentData>>
    {
        [SerializeField] private bool _countBlood;
        [SerializeField] private bool _countSteam;
        [SerializeField] private bool _mustBeSimultaneous;

        protected ReceiverQuest(string name, string description, int target, bool countBlood, bool countSteam,
            bool mustBeSimultaneous)
            : base(name, description, target)
        {
            _countBlood = countBlood;
            _countSteam = countSteam;
            _mustBeSimultaneous = mustBeSimultaneous;
        }

        public override Quest<IEnumerable<SegmentData>> Build(int target) =>
            new ReceiverQuest(Name, Description, target, _countBlood, _countSteam, _mustBeSimultaneous);

        public override void Progress(IEnumerable<SegmentData> segments)
        {
            var sources = segments.Where(segment => segment.StaticSegmentData.IsSource).ToList();

            if (_countBlood)
            {
                var blood = sources.Count(source => source.StaticSegmentData.IsBlood);
                Count += blood;
            }

            if (_countSteam)
            {
                var steam = sources.Count(source => source.StaticSegmentData.IsSteam);
                Count += steam;
            }

            if (Count >= Target)
            {
                Complete();
            }

            if (_mustBeSimultaneous)
            {
                Count = 0;
            }

            UpdateDescription();
        }
    }

    [Serializable]
    public class PlaceSourceQuest : Quest<SegmentData>
    {
        [SerializeField] private bool _needsBlood;
        [SerializeField] private bool _needsSteam;
        protected PlaceSourceQuest(string name, string description, bool needsBlood, bool needsSteam) : base(name, description, target: 1)
        {
            _needsBlood = needsBlood;
            _needsSteam = needsSteam;
        }
        public override Quest<SegmentData> Build(int target) => new PlaceSourceQuest(Name, Description, _needsBlood, _needsSteam);
        
        public override void Progress(SegmentData segment)
        {
            if (_needsBlood && segment.StaticSegmentData.IsBlood && segment.StaticSegmentData.IsSource)
            {
                Complete();
            }
            else if (_needsSteam && segment.StaticSegmentData.IsSteam && segment.StaticSegmentData.IsSource)
            {
                Complete();
            }
        }
    }

    [Serializable]
    public class ResourcesQuest : Quest<(int, int)>
    {
        [SerializeField] private bool _countBlood;
        [SerializeField] private bool _countSteam;

        protected ResourcesQuest(string name, string description, int target, bool countBlood, bool countSteam)
            : base(name, description, target)
        {
            _countBlood = countBlood;
            _countSteam = countSteam;
        }

        public override Quest<(int, int)> Build(int target) =>
            new ResourcesQuest(Name, Description, target, _countBlood, _countSteam);

        public override void Progress((int, int) resources)
        {
            Count = _countBlood ? resources.Item1 : resources.Item2;

            if (Count >= Target)
            {
                Complete();
            }

            UpdateDescription();
        }
    }

    [Serializable]
    public class MeasureQuest : Quest
    {
        protected float Count;
        [SerializeField] protected float Target;
        
        protected MeasureQuest(string name, string description, float target) : base(name, description)
        {
            Target = target;
        }
        
        public override Quest Build() => new MeasureQuest(Name, Description, Target);

        public void Progress(float amount)
        {
            Count += amount;

            if (Count >= Target)
            {
                Complete();
            }
        }
    }
}