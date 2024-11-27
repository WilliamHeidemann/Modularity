using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Runtime.Scriptable_Objects
{
    [Serializable]
    public class Quest
    {
        [SerializeField] [TextArea(3, 10)] protected string Description;
        public bool IsCompleted { get; protected set; }
        public void Complete() => IsCompleted = true;

        public Quest Build()
        {
            return new Quest
            {
                Description = Description
            };
        }
    }

    [Serializable]
    public class Quest<T> : Quest
    {
        protected int Count;
        protected int Target;

        public virtual Quest<T> Build(int x)
        {
            return new Quest<T>
            {
                Description = Description,
                Target = x
            };
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
                IsCompleted = true;
            }
        }
    }

    [Serializable]
    public class SegmentQuest : Quest<SegmentData>
    {
        [SerializeField] private bool _countBlood;
        [SerializeField] private bool _countSteam;

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
                IsCompleted = true;
            }
        }
    }


    [Serializable]
    public class ReceiverQuest : Quest<IEnumerable<SegmentData>>
    {
        [SerializeField] private bool _countBlood;
        [SerializeField] private bool _countSteam;
        [SerializeField] private bool _mustBeSimultaneous;

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
                IsCompleted = true;
            }

            if (_mustBeSimultaneous)
            {
                Count = 0;
            }
        }
    }

    [Serializable]
    public class ResourcesQuest : Quest<(int, int)>
    {
        [SerializeField] private bool _countBlood;
        [SerializeField] private bool _countSteam;

        public override void Progress((int, int) resources)
        {
            Count = _countBlood ? resources.Item1 : resources.Item2;

            if (Count >= Target)
            {
                IsCompleted = true;
            }
        }
    }
}