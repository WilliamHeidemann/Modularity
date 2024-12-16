using Runtime.Components;
using UnityEngine;

public class StartUpSequenceController : MonoBehaviour
{
    public static bool startUpHasPlayed = false;
    private Animator _startUpAnimator;
    public OptionMenuController _optionMenuController;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _optionMenuController.GameStartupSettings();

        if (!startUpHasPlayed)
        {
            // Play the start up sequence
            _startUpAnimator = GetComponent<Animator>();
            _startUpAnimator.Play("Base Layer.Start-up Animation");
            startUpHasPlayed = true;
        }
    }
}
