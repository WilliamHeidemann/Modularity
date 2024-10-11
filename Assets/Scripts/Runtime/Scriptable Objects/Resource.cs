using UnityEngine;

namespace Runtime.Scriptable_Objects
{
    [CreateAssetMenu(fileName = "New Resource", menuName = "Resource", order = 0)]
    public class Resource : ScriptableObject
    {
        public int value;
    }
}