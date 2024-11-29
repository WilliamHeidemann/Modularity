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
        [SerializeField] private Builder _builder;
        [SerializeField] private Structure _structure;
        [SerializeField] private Hand _hand;
        [SerializeField] private FlowControl _flowControl;
        [SerializeField] private Selection _selection;
        [SerializeField] private Segment _bloodSource;
        [SerializeField] private Segment _steamSource;
        [SerializeField] private Currency _currency;
        [FormerlySerializedAs("_automaticSourceSpawning")] [SerializeField] private AutoSpawner _autoSpawner;
        [SerializeField] private QuestFactory _questFactory;
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
            _structure.Clear();
            _flowControl.Clear();
            _hand.Initialize();
            _currency.Initialize(_startingCurrency);
            _questFactory.OnCameraCompleted += _autoSpawner.SpawnBloodSource;
        }

        // private void Update()
        // {
        //     if (Input.GetKeyDown(KeyCode.N))
        //     {
        //         _automaticSourceSpawning.SpawnSteamSource();
        //     }
        // }
    }
}