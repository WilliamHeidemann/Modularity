using Runtime.Components.Segments;
using UnityEngine;

public class Pipe : MonoBehaviour
{
    [SerializeField] private Segment _segment;
    
    [SerializeField] private GameObject _upPart;
    [SerializeField] private GameObject _downPart;
    [SerializeField] private GameObject _rightPart;
    [SerializeField] private GameObject _leftPart;
    [SerializeField] private GameObject _frontPart;
    [SerializeField] private GameObject _backPart;

    void Start() => EnableParts();

    private void EnableParts()
    {
        _upPart.SetActive(_segment.StaticSegmentData.ConnectionPoints.Up);
        _downPart.SetActive(_segment.StaticSegmentData.ConnectionPoints.Down);
        _rightPart.SetActive(_segment.StaticSegmentData.ConnectionPoints.Right);
        _leftPart.SetActive(_segment.StaticSegmentData.ConnectionPoints.Left);
        _frontPart.SetActive(_segment.StaticSegmentData.ConnectionPoints.Forward);
        _backPart.SetActive(_segment.StaticSegmentData.ConnectionPoints.Back);
    }
}
