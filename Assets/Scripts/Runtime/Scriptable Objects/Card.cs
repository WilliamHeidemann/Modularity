using System;
using Runtime.Models;
using UnityEngine;
using UnityEngine.Serialization;

namespace Runtime.Scriptable_Objects
{
    [CreateAssetMenu(menuName = "Card")]
    public class Card : ScriptableObject
    {
        [field: SerializeField] public Supply Supply { get; private set; }
        [field: SerializeField] public Model Model { get; private set; }
    }
}