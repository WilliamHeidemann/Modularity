using Runtime.Scriptable_Objects;
using UnityEngine;
using Runtime.DataLayer;

public class TestingParticles : MonoBehaviour
{
    private float _flowTimer = 1;
    private float _burstTimer = 5;
    [SerializeField] private ParticleManager particleManager;

    private void Start()
    {
        particleManager.Initialize();
        particleManager.SpawnParticleFX(ParticleType.BloodFlow, transform.position + new Vector3(Random.Range(0, 3), Random.Range(0, 3), Random.Range(0, 3)), transform.rotation, true);
        particleManager.SpawnParticleFX(ParticleType.BloodFlow, transform.position + new Vector3(Random.Range(0, 3), Random.Range(0, 3), Random.Range(0, 3)), transform.rotation, true);
        particleManager.SpawnParticleFX(ParticleType.SteamFlow, transform.position + new Vector3(Random.Range(0, 3), Random.Range(0, 3), Random.Range(0, 3)), transform.rotation, true);
        particleManager.SpawnParticleFX(ParticleType.SteamFlow, transform.position + new Vector3(Random.Range(0, 3), Random.Range(0, 3), Random.Range(0, 3)), transform.rotation, true);
    }

    // Update is called once per frame
    void Update()
    {
        if (_flowTimer > 0)
        {
            _flowTimer -= Time.deltaTime;
        }
        else
        {
            _flowTimer = 1.5f;
            particleManager.MoveParticleFX(transform.position + new Vector3(Random.Range(0, 3), Random.Range(0, 3), Random.Range(0, 3)), Quaternion.Euler(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360)));
        }

        if (_burstTimer > 0)
        {
            _burstTimer -= Time.deltaTime;
        }
        else
        {
            _burstTimer = Random.Range(3, 8);

            int randomBurst = Random.Range(0, 3);
            if (randomBurst == 0)
            {
                particleManager.SpawnParticleFX(ParticleType.FleshBurst, transform.position + new Vector3(Random.Range(0, 3), Random.Range(0, 3), Random.Range(0, 3)), transform.rotation, false);
            }
            else if (randomBurst == 1)
            {
                particleManager.SpawnParticleFX(ParticleType.GearBurst, transform.position + new Vector3(Random.Range(0, 3), Random.Range(0, 3), Random.Range(0, 3)), transform.rotation, false);
            }
            else
            {
                particleManager.SpawnParticleFX(ParticleType.HybridBurst, transform.position + new Vector3(Random.Range(0, 3), Random.Range(0, 3), Random.Range(0, 3)), transform.rotation, false);
            }
        }
    }
}
