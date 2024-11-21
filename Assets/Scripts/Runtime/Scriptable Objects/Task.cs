using Runtime.Components.Segments;
using UnityEngine;
using System;
using System.Collections.Generic;

namespace Runtime.Scriptable_Objects
{
    [Serializable]
    public class Task
    {
        public string Name;
        [TextArea(3, 10)]
        public string Description;
        public bool IsCompleted;
    }
}
