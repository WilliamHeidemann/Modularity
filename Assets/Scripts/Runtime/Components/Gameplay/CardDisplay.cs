using System;
using Runtime.Models;
using Runtime.Scriptable_Objects;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Runtime.Components.Gameplay
{
    public class CardDisplay : MonoBehaviour, IPointerDownHandler
    {
        [SerializeField] private TextMeshProUGUI _name;
        [SerializeField] private TextMeshProUGUI _blood;
        [SerializeField] private TextMeshProUGUI _energy;
        [SerializeField] private TextMeshProUGUI _mechanical;

        public static event Action<CardScriptableObject> OnCardSelect;
        public CardScriptableObject Card { get; private set; }

        public void Init(CardScriptableObject card)
        {
            Card = card;
        }
        
        private void Start()
        {
            if (Card == null)
            {
                Debug.LogError("CardDisplay was instantiated without calling Init!");
                return;
            }
            _name.text = Card.Supply.ToString();
            _blood.text = $"Blood: {Card.Supply.Blood.ToString()}";
            _energy.text = $"Energy: {Card.Supply.Energy.ToString()}";
            _mechanical.text = $"Mechanical: {Card.Supply.Mechanical.ToString()}";
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            OnCardSelect?.Invoke(Card);
        }
    }
}
