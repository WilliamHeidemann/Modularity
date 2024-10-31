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
        _upPart.SetActive(_segment.ConnectionPoints.Up);
        _downPart.SetActive(_segment.ConnectionPoints.Down);
        _rightPart.SetActive(_segment.ConnectionPoints.Right);
        _leftPart.SetActive(_segment.ConnectionPoints.Left);
        _frontPart.SetActive(_segment.ConnectionPoints.Forward);
        _backPart.SetActive(_segment.ConnectionPoints.Back);
    }
}
