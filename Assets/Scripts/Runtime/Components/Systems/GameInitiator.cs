using Runtime.DataLayer;
using Runtime.Scriptable_Objects;
using UnityEngine;

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
        [SerializeField] private QuestController _questController;
        [SerializeField] private GameOverMenuController _gameOverMenuController;
        [SerializeField] private EndGame _endGame;
        [SerializeField] private PlaceHolderBuilder _placeHolderBuilder;
        [SerializeField] private SlotVisualizer _slotVisualizer;
        

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
            _questFactory.Clear();
            _hand.Clear();
            _structure.Clear();
            _autoSpawner.Clear();
            _scoreTracker.Clear();
            _placeHolderBuilder.Clear();
            _currency.Initialize(_startingCurrency);
            _questController.Initialize();
            _slotVisualizer.Initialize();
            _endGame.SetGameOverMenu(_gameOverMenuController);
        }
    }
}