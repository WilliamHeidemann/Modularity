using Runtime.Models;
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

        public Card Card { get; private set; }

        public void Init(Card card)
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
            _name.text = Card.Model.ToString();
            _blood.text = $"Blood: {Card.SegmentData.Blood.ToString()}";
            _energy.text = $"Energy: {Card.SegmentData.Energy.ToString()}";
            _mechanical.text = $"Mechanical: {Card.SegmentData.Mechanical.ToString()}";
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            print("Card selected");
            SegmentFactory.Instance.Select(Card);
        }
    }
}
