using DG.Tweening;
using System.Collections;
using UnityEngine;

public class TheWatcherSystem : MonoBehaviour
{
    private Transform _cameraTransform;
    private Vector3 _lookPosition;
    [SerializeField] private float _hoverHeight;
    [SerializeField] private float _hoverSpeedUp;
    [SerializeField] private float _hoverSpeedDown;
    private float _currentLockOnSpeed;
    [SerializeField] private float _lockOnSpeed;

    [Header("Eye Flickering")]
    [SerializeField] private bool _eyeFlicker = true; //disables eye flicker when false;
    [SerializeField] private float _lockOnSpeedFlickering;
    [SerializeField] private float _flickeringStrength;
    [SerializeField] private float _randomIntervalLOW;
    [SerializeField] private float _randomIntervalHIGH;
    [SerializeField] private int _flickeringAmountLOW;
    [SerializeField] private int _flickeringAmountHIGH;
    private float randomnessTimer = 10;
    private bool isEyeFlickering = false;

    void Start()
    {
        _currentLockOnSpeed = _lockOnSpeed;
        _cameraTransform = transform.parent;
        transform.parent = null;
        transform.position = new Vector3(0, _hoverHeight + _cameraTransform.position.y, 0);
    }

    void Update()
    {
        float hoverSpeed = 0;

        if(_cameraTransform.position.y + _hoverHeight < transform.position.y)
        {
            hoverSpeed = _hoverSpeedDown;
        }
        else if (_cameraTransform.position.y + _hoverHeight > transform.position.y)
        {
            hoverSpeed = _hoverSpeedUp;
        }

        transform.position = Vector3.Slerp(transform.position, new Vector3(0, _hoverHeight + _cameraTransform.position.y, 0), Time.deltaTime * hoverSpeed);
        
        if(!isEyeFlickering)
        {
            _lookPosition = _cameraTransform.position;
        }

        Quaternion lookOnLook = Quaternion.LookRotation(_lookPosition - transform.position); 
        transform.rotation = Quaternion.Slerp(transform.rotation, lookOnLook, Time.deltaTime * _currentLockOnSpeed);

        if(!_eyeFlicker)
        {
            return;
        }

        if (randomnessTimer <= 0 && !isEyeFlickering)
        {
            StartCoroutine(RandomEyeFlickering());
        }
        else
        {
            randomnessTimer -= Time.deltaTime;
        }
    }

    IEnumerator RandomEyeFlickering()
    {
        isEyeFlickering = true;
        _currentLockOnSpeed = _lockOnSpeedFlickering;
        int flickerAmount = Random.Range(_flickeringAmountLOW, _flickeringAmountHIGH);

        for (int i = 0; i < flickerAmount; i++)
        {
            if(flickerAmount - 1 == i)
            {
                _lookPosition = _cameraTransform.position + Random.insideUnitSphere * _flickeringStrength * 3;
            }
            else
            {
                _lookPosition = _cameraTransform.position + Random.insideUnitSphere * _flickeringStrength;
            }
            yield return new WaitForSeconds(Random.Range(0.05f, 0.15f));
        }

        isEyeFlickering = false;
        _currentLockOnSpeed = _lockOnSpeed;
        randomnessTimer = Random.Range(_randomIntervalLOW, _randomIntervalHIGH);
    }
}
