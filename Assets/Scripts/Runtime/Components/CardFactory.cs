using System.Linq;
using Runtime.Models;
using UnityEngine;
using UnityEngine.Serialization;
using UtilityToolkit.Runtime;

namespace Runtime.Components
{
    public class CardFactory : MonoBehaviour
    {
        [SerializeField] private CardDisplay _cardDisplayPrefab;

        public CardDisplay RandomCard()
        {
            var model = For.GetValues<Model>().Except(new[] { Model.ConnectionSlot }).RandomElement();
            var card = new Card(model);
            var cardDisplay = Instantiate(_cardDisplayPrefab);
            cardDisplay.Init(card);
            return cardDisplay;
        }
    }
}