using System;
using DG.Tweening;
using Runtime.Components.Segments;
using Runtime.Components.Utility;
using Runtime.Scriptable_Objects;
using UnityEngine;
using UnityEngine.Serialization;
using UnityUtils;
using UtilityToolkit.Runtime;

namespace Runtime.Components.Systems
{
    public class GameInitiator : MonoBehaviour
    {
        [SerializeField] private Structure _structure;
        [SerializeField] private Hand _hand;
        [SerializeField] private Currency _currency;
        [SerializeField] private AutoSpawner _autoSpawner;
        [SerializeField] private QuestFactory _questFactory;
        [SerializeField] private ScoreTracker _scoreTracker;
        [SerializeField] private int _startingCurrency;

        private void OnEnable()
        {
            MainMenuController.OnGameStart += Initialize;
        }

        private void OnDisable()
        {
            MainMenuController.OnGameStart -= Initialize;
        }

        private void Initialize()
        {
            DOTween.Init();
            _hand.Clear();
            _structure.Clear();
            _autoSpawner.Clear();
            _scoreTracker.Clear();
            _currency.Initialize(_startingCurrency);
        }

        // private void Update()
        // {
        //     if (Input.GetKeyDown(KeyCode.N))
        //     {
        //         _autoSpawner.SpawnCollectable();
        //     }
        // }
    }
}