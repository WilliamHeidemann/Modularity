using UnityEngine;
using System;
using DG.Tweening;
using Runtime.Components.Segments;
using Runtime.Components.Utility;
using Runtime.Scriptable_Objects;
using UnityEngine.Serialization;
using UnityUtils;
using UtilityToolkit.Runtime;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;

[CreateAssetMenu(fileName = "ScoreTracker", menuName = "ScoreTracker")]
public class ScoreTracker : ScriptableObject
{
    [SerializeField] private int _heartsConnected;
    [SerializeField] private int _furnacesConnected;
    [SerializeField] private int _brainsActivated;
    [SerializeField] private int _enginesActivated;
    [SerializeField] private int _hybridsActivated;
    [SerializeField] private int _energySpheresCollected;
    void OnEnable()
    {
        ResetScore();
        FlowControl.OnProducerActivated += ProducerActivation;
        FlowControl.OnSourcesLinkedCheck += UpdateSourceScore;
        AutoSpawner.OnCollectedCollectables += Collected;
    }
    void OnDisable()
    {
        FlowControl.OnProducerActivated -= ProducerActivation;
        FlowControl.OnSourcesLinkedCheck -= UpdateSourceScore;
        AutoSpawner.OnCollectedCollectables -= Collected;

    }
    private void ProducerActivation(StaticSegmentData staticSegmentData)
    {
        if (staticSegmentData.IsBlood && staticSegmentData.IsSteam)
        {
            _hybridsActivated += 1;
        }
        else if (staticSegmentData.IsBlood)
        {
            _brainsActivated += 1;
        }
        else _enginesActivated +=1;
    }
    private void UpdateSourceScore(HashSet<SegmentData> sources)
    {
        var blood = 0;
        var steam = 0;
        foreach (var source in sources)
        {
            if (source.StaticSegmentData.IsBlood)
            {
                blood += 1;
            }
            else steam += 1;
        }
        _heartsConnected = blood;
        _furnacesConnected = steam;
    }
    private void Collected(int noCollected)
    {
        _energySpheresCollected += noCollected;
    }
    private void ResetScore()
    {
        _heartsConnected = 0;
        _furnacesConnected = 0;
        _brainsActivated = 0;
        _enginesActivated = 0;
        _energySpheresCollected = 0;
    }

}
