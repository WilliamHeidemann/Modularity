using UnityEngine;

namespace Runtime.Components.Systems
{
    public class GameInitiator : MonoBehaviour
    {
        [SerializeField] private Transform _sceneCanvas;
        [SerializeField] private StructureManager _structureManager;
        [SerializeField] private CardManager _cardManager;
        private void Start()
        {
            Instantiate(_structureManager);
            Instantiate(_cardManager, _sceneCanvas);
        }
    }
}
