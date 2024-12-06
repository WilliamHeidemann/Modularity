using System;
using System.Collections.Generic;
using Runtime.Components.Segments;
using Runtime.Scriptable_Objects;
using UnityEngine;

[CreateAssetMenu(fileName = "EndGame", menuName = "Scriptable Objects/EndGame")]
public class EndGame : ScriptableObject
{
    [SerializeField] private Structure _structure;
    [SerializeField] private Currency _currency;
    private GameOverMenuController _gameOverMenu;

    public void SetGameOverMenu(GameOverMenuController gameOverMenuController) => _gameOverMenu = gameOverMenuController;
    public void CheckHand(List<Segment> segments, int rerollCostBlood, int rerollCostSteam)
    {
        if (_currency.BloodAmount < rerollCostBlood && !HasOpenSlotOfType(IsSteam)) _gameOverMenu.gameObject.SetActive(true);
        if (_currency.SteamAmount < rerollCostSteam && !HasOpenSlotOfType(IsBlood)) _gameOverMenu.gameObject.SetActive(true);
        if (CanAffordCard(segments, rerollCostBlood, rerollCostSteam) && HasOpenSlots()) return;
    
    }

    private bool CanAffordCard(List<Segment> segments, int rerollCostBlood, int rerollCostSteam)
    {
        if (_currency.BloodAmount >= rerollCostBlood && _currency.SteamAmount >= rerollCostSteam) return true;
        foreach (var segment in segments)
        {
            if (segment.StaticSegmentData.BloodCost > 0 && segment.StaticSegmentData.SteamCost > 0)
            {
                if (_currency.BloodAmount - segment.StaticSegmentData.BloodCost > -1 &&
                    _currency.SteamAmount - segment.StaticSegmentData.SteamCost > -1) return true;
            }
            else if (segment.StaticSegmentData.BloodCost > 0 &&
                        _currency.BloodAmount - segment.StaticSegmentData.BloodCost > -1) return true;
            else if (segment.StaticSegmentData.SteamCost > 0 &&
                        _currency.SteamAmount - segment.StaticSegmentData.SteamCost > -1) return true;
        }

        return false;
    }
    private bool HasOpenSlots()
    {
        foreach (var segment in _structure.Segments)
        {
            foreach (var link in segment.GetConnectionPoints())
            {
                if (_structure.IsOpenPosition(link)) return true;
            }
        }
        return false;
    }
    private bool HasOpenSlotOfType(Predicate<ConnectionType> predicate)
    {
        foreach (var segment in _structure.Segments)
        {
            foreach (var link in segment.GetConnectionPointsPlus())
            {
                if (_structure.IsOpenPosition(link.Item1) && predicate(link.Item2)) 
                {
                    Debug.Log(link.Item2);
                    Debug.Log(predicate(link.Item2));
                    return true;
                }
            }
        }
        return false;
    }
    private bool IsBlood(ConnectionType connection) => connection == ConnectionType.Blood;
    private bool IsSteam(ConnectionType connection) => connection == ConnectionType.Steam;
    private bool IsAny(ConnectionType connection) => true;

}
