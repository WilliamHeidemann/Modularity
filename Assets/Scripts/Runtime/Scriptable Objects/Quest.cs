using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using TMPro;

namespace Runtime.Scriptable_Objects
{
    [Serializable]
    public class Quest
    {
        [field: SerializeField]
        [TextArea(3, 10)]
        public string Description { get; protected set; }

        [field: SerializeField]
        [TextArea(3, 10)]
        public string Explanation { get; protected set; }

        public Action OnComplete;
        public TextMeshProUGUI DescriptionText;

        protected Quest(string description, string explanation)
        {
            Description = description;
            Explanation = explanation;
        }

        public void Complete()
        {
            OnComplete?.Invoke();
        }

        public Quest Build() => new(Description, Explanation);
    }

    [Serializable]
    public class Quest<T> : Quest
    {
        protected int Count;
        protected int Target;

        protected Quest(string description, string explanation, int target) : base(description, explanation)
        {
            Target = target;
            Description = Regex.Replace(Description, @"/\d", $"/{target}");
            Explanation = Regex.Replace(Explanation, @"\b x \b", $" {target} ");
        }

        public virtual Quest<T> Build(int target) => new(Description, Explanation, target);

        protected void UpdateDescription()
        {
            Description = Regex.Replace(Description, @"\(\d+/2\)", $"({Count}/2)");
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

        protected SegmentQuest(string description, string explanation, int target, bool countBlood, bool countSteam)
            : base(description, explanation, target)
        {
            _countBlood = countBlood;
            _countSteam = countSteam;
        }

        public override Quest<SegmentData> Build(int target) =>
            new SegmentQuest(Description, Explanation, target, _countBlood, _countSteam);

        public override void Progress(SegmentData segments)
        {
            if (_countBlood && segments.StaticSegmentData.IsBlood)
            {
                Count++;
            }

            if (_countSteam && segments.StaticSegmentData.IsSteam)
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

        protected ReceiverQuest(string description, string explanation, int target, bool countBlood, bool countSteam,
            bool mustBeSimultaneous)
            : base(description, explanation, target)
        {
            _countBlood = countBlood;
            _countSteam = countSteam;
            _mustBeSimultaneous = mustBeSimultaneous;
        }

        public override Quest<IEnumerable<SegmentData>> Build(int target) =>
            new ReceiverQuest(Description, Explanation, target, _countBlood, _countSteam, _mustBeSimultaneous);

        public override void Progress(IEnumerable<SegmentData> segments)
        {
            var receivers = segments.Where(segment => segment.StaticSegmentData.IsReceiver).ToList();

            if (_countBlood)
            {
                var blood = receivers.Count(receiver => receiver.StaticSegmentData.IsBlood);
                Count += blood;
            }

            if (_countSteam)
            {
                var steam = receivers.Count(receiver => receiver.StaticSegmentData.IsSteam);
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
    public class ResourcesQuest : Quest<(int, int)>
    {
        [SerializeField] private bool _countBlood;
        [SerializeField] private bool _countSteam;

        protected ResourcesQuest(string description, string explanation, int target, bool countBlood, bool countSteam)
            : base(description, explanation, target)
        {
            _countBlood = countBlood;
            _countSteam = countSteam;
        }

        public override Quest<(int, int)> Build(int target) =>
            new ResourcesQuest(Description, Explanation, target, _countBlood, _countSteam);

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
}