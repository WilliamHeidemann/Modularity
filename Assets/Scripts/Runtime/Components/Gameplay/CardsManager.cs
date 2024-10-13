using System.Collections.Generic;
using Runtime.Models;
using Runtime.Scriptable_Objects;
using UnityEngine;
using UtilityToolkit.Runtime;

namespace Runtime.Components.Gameplay
{
    public class CardsManager : MonoSingleton<CardsManager>
    {
        [SerializeField] private CardFactory _factory;
        [SerializeField] private List<CardDisplay> _cards = new();
        [SerializeField] private Transform _hand;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.H))
            {
                ReplaceHand();
            }
        }

        private void ReplaceHand()
        {
            // SegmentFactory.Instance.Deselect();
            
            foreach (var card in _cards)
            {
                Destroy(card.gameObject, 0.1f);
            }
            _cards.Clear();

            for (int i = 0; i < 4; i++)
            {
                DrawCard();
            }
        }

        public void DrawCard()
        {
            var card = _factory.RandomCard();
            card.transform.SetParent(_hand);
            _cards.Add(card);
        }

        public void SpendCard(CardScriptableObject card)
        {
            var displayOption = _cards.FirstOption(display => display.Card == card);
            if (displayOption.IsSome(out var cardDisplay))
            {
                _cards.Remove(cardDisplay);
                Destroy(cardDisplay.gameObject);   
            }
        }
    }
}