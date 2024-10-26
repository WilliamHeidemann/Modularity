using UnityEngine;
using UnityEngine.Serialization;

namespace Runtime.Scriptable_Objects
{
    [CreateAssetMenu]
    public class Shop : ScriptableObject
    {
        // Contains information of what the player can choose between
        // as well as the cost for each option. 

        // Has a bank of prefabs to select from with various probabilities 
        // for each segment, that are easily modified by a designer. 

        // When the player has clicked the UI element representing their 
        // chosen prefab, and the player has enough resources, the 
        // selected segment will change to this segment prefab. 

        [SerializeField] private Selection _selection;
        [SerializeField] private Resources _resources;
    }
}