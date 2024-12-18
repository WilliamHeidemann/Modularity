using UnityEngine;

public class CreditsMenuController : MonoBehaviour
{
    private bool _isCreditsActive = false;
    private Animator _creditsAnimator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnEnable()
    {
        _isCreditsActive = true;
        _creditsAnimator = GetComponent<Animator>();
        _creditsAnimator.speed = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_isCreditsActive)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            _creditsAnimator.Play("Exit Layer.Credits Roll Exit Animation");
            _isCreditsActive = false;
        }
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            _creditsAnimator.speed = 5;
        }
        else if (Input.GetKeyUp(KeyCode.Space) || Input.GetMouseButtonUp(0))
        {
            _creditsAnimator.speed = 1;
        }
    }

    public void OnCreditsExit()
    {
        _isCreditsActive = false;
        gameObject.SetActive(false);
    }
}
