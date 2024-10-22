using System;
using Runtime.Models;
using Runtime.Scriptable_Objects;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Runtime.Components.Systems
{
    public class MonoCard : MonoBehaviour, IPointerDownHandler
    {
        [SerializeField] private TextMeshProUGUI _name;

        public static event Action<Model> OnCardSelect;
        public Model Model { get; private set; }

        public void Init(Model model)
        {
            Model = model;
        }
        
        private void Start()
        {
            _name.text = Model.ToString();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            OnCardSelect?.Invoke(Model);
        }
    }
}
