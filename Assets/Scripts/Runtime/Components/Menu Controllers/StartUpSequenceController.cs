using UnityEngine;

public class StartUpSequenceController : MonoBehaviour
{
    public static bool startUpHasPlayed = false;
    private Animator _startUpAnimator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (!startUpHasPlayed)
        {
            // Play the start up sequence
            _startUpAnimator = GetComponent<Animator>();
            _startUpAnimator.Play("Base Layer.Start-up Animation");
            startUpHasPlayed = true;
        }
    }
}
