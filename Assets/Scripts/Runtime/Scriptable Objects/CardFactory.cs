using System.Linq;
using Runtime.Components.Systems;
using Runtime.Models;
using UnityEngine;
using UnityEngine.Serialization;
using UtilityToolkit.Runtime;

namespace Runtime.Scriptable_Objects
{
    [CreateAssetMenu(menuName = "Card Factory")]
    public class CardFactory : ScriptableObject
    {
        [SerializeField] private MonoCard _monoCardPrefab;
        [SerializeField] private Card[] _cards;
        
        public MonoCard RandomCard()
        {
            var model = For.GetValues<Model>().Except(new[] { Model.ConnectionSlot }).RandomElement();
            var card = _cards.First(card => card.Model == model);
            var cardDisplay = Instantiate(_monoCardPrefab);
            cardDisplay.Init(card);
            return cardDisplay;
        }
    }
}