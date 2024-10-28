using Runtime.Components.Segments;
using Runtime.Scriptable_Objects;
using TMPro;
using UnityEngine;

public class ShopUI : MonoBehaviour
{
    [SerializeField] private Shop _shop;
    [SerializeField] private GameObject _shopOption;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        DisplayOptions();
    }
    
    void DisplayOptions()
    {
        int displayPosition = 0;

        foreach (Segment segment in _shop._segmentsOptions)
        {
            GameObject shopOptionClone = Instantiate(_shopOption, transform.position, Quaternion.identity);
            shopOptionClone.transform.SetParent(transform);
            shopOptionClone.transform.localPosition = new Vector3(displayPosition, 0, 0);
            displayPosition += 200;
        }
    }
}
