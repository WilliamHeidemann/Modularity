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
        [FormerlySerializedAs("_monoCardPrefab")] [SerializeField] private Card _cardPrefab;
        
        public Card RandomCard()
        {
            // var model = For.GetValues<Model>().RandomElement();
            var model = Model.ConnectorBox;
            var cardDisplay = Instantiate(_cardPrefab);
            cardDisplay.Init(model);
            return cardDisplay;
        }
    }
}