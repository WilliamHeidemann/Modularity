using System.Linq;
using Runtime.Models;
using Runtime.Scriptable_Objects;
using UnityEngine;
using UtilityToolkit.Runtime;

namespace Runtime.Components.Gameplay
{
    public class CardFactory : MonoBehaviour
    {
        [SerializeField] private CardDisplay _cardDisplayPrefab;
        [SerializeField] private CardScriptableObject[] _cards;
        
        public CardDisplay RandomCard()
        {
            var model = For.GetValues<Model>().Except(new[] { Model.ConnectionSlot }).RandomElement();
            var card = _cards.First(card => card.Model == model);
            var cardDisplay = Instantiate(_cardDisplayPrefab);
            cardDisplay.Init(card);
            return cardDisplay;
        }
    }
}