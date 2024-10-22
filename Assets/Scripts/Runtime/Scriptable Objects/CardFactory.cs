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
        
        public MonoCard RandomCard()
        {
            // var model = For.GetValues<Model>().RandomElement();
            var model = Model.ConnectorBox;
            var cardDisplay = Instantiate(_monoCardPrefab);
            cardDisplay.Init(model);
            return cardDisplay;
        }
    }
}