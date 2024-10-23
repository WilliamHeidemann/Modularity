using System;
using System.Collections.Generic;
using Runtime.Models;
using Runtime.Scriptable_Objects;
using UnityEngine;
using UtilityToolkit.Runtime;

namespace Runtime.Components.Systems
{
    public class CardManager : MonoSingleton<CardManager>
    {
        [SerializeField] private CardFactory _factory;
        [SerializeField] private List<Card> _cards = new();
        public static event Action OnHandReplaced;

        private void Start()
        {
            OnHandReplaced += ReplaceHand;
        }

        private void OnDisable()
        {
            OnHandReplaced -= ReplaceHand;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.H))
            {
                OnHandReplaced?.Invoke();
            }
        }

        private void ReplaceHand()
        {
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
            card.transform.SetParent(transform);
            _cards.Add(card);
        }

        public void SpendCard(Model model)
        {
            var displayOption = _cards.FirstOption(display => display.Model == model);
            if (displayOption.IsSome(out var cardDisplay))
            {
                _cards.Remove(cardDisplay);
                Destroy(cardDisplay.gameObject);   
            }
        }
    }
}