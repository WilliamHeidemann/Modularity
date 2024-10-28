using Runtime.Scriptable_Objects;
using UnityEngine;

public class ShopUI : MonoBehaviour
{
    [SerializeField] private Shop _shop;
    [SerializeField] private GameObject _shopOption;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _shop.GenerateOptions();
        DisplayOptions();
    }
    
    void DisplayOptions()
    {
        int displayPosition = 0;

        // Display the options to the player
        for (int i = 0; i <= _shop._optionsCount; i++)
        {
            var displayOption = Instantiate(_shopOption, transform.position, Quaternion.identity);
            displayOption.transform.position = new Vector3(displayPosition, 0, 0);

            displayPosition += 200;
        }
    }
}
