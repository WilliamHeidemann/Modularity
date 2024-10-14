using Runtime.Models;
using UnityEngine;
using UnityEngine.Serialization;

namespace Runtime.Scriptable_Objects
{
    [CreateAssetMenu(menuName = "Card")]
    public class Card : ScriptableObject
    {
        [SerializeField] private string _title;
        [SerializeField] private Supply _supply;
        [SerializeField] private Model _model;

        public string Title => _title;
        public Supply Supply => _supply;
        public Model Model => _model;
    }
}