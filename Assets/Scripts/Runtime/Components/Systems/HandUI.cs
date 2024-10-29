using Runtime.Components;
using Runtime.Components.Segments;
using Runtime.Scriptable_Objects;
using TMPro;
using UnityEngine;

public class HandUI : MonoBehaviour
{
    [SerializeField] private Blueprint[] _blueprintOptions;
    [SerializeField] private Hand _hand;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        DisplayOptions();
    }
    
    void DisplayOptions()
    {
        Segment[] segments = _hand._segmentsOptions;

        for (int i = 0; i < _blueprintOptions.Length; i++)
        {
            _blueprintOptions[i].SetName(segments[i].name);
            _blueprintOptions[i].SetCost(segments[i].ConnectionPoints.OpenConnectionPoints().ToString());
        }
    }
}
