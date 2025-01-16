using System;
using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using Runtime.DataLayer;
using Codice.CM.Client.Differences.Merge;
using Random = UnityEngine.Random;

namespace Runtime.Scriptable_Objects
{
    [CreateAssetMenu]
    public class ParticleManager : ScriptableObject
    {
        [SerializeField] private GameObject _bloodFlowFX;
        [SerializeField] private GameObject _steamFlowFX;
        [SerializeField] private GameObject _fleshBurstFX;
        [SerializeField] private GameObject _gearBurstFX;
        [SerializeField] private GameObject _hybridBurstFX;

        private List<GameObject> activeBloodFlowFX = new List<GameObject>();
        private List<GameObject> activeSteamFlowFX = new List<GameObject>();

        //clear both lists must be called at the start of the game.
        public void Initialize()
        {
            activeBloodFlowFX.Clear();
            activeSteamFlowFX.Clear();
        }

        // This method is used to spawn a particle effect at a given position and rotation, for any of the burst FX the rotation will have no effect on the particles spawned.
        public void SpawnParticleFX(ParticleType particleType, Vector3 spawnPosition, Quaternion spawnRotation,
            bool isPermanent)
        {
            GameObject prefab = particleType switch
            {
                ParticleType.BloodFlow => _bloodFlowFX,
                ParticleType.SteamFlow => _steamFlowFX,
                ParticleType.FleshBurst => _fleshBurstFX,
                ParticleType.GearBurst => _gearBurstFX,
                ParticleType.HybridBurst => _hybridBurstFX,
                _ => null
            };

            GameObject spawnedFX = Instantiate(prefab, spawnPosition, spawnRotation);

            if (!isPermanent)
            {
                Destroy(spawnedFX, 5f);
            }
            else
            {
                if (particleType == ParticleType.BloodFlow)
                {
                    activeBloodFlowFX.Add(spawnedFX);
                }
                else if (particleType == ParticleType.SteamFlow)
                {
                    activeSteamFlowFX.Add(spawnedFX);
                }
                else
                {
                    Debug.LogError("Particle type is not meant to be a permanent type!");
                }
            }
        }

        public void MoveParticleFX(Vector3 newPosition, Quaternion newRotation)
        {
            List<GameObject> targetFXList = null;
            float randomNumber = Random.Range(0f, 1f);

            if (randomNumber < 0.5f)
            {
                targetFXList = activeBloodFlowFX;
            }
            else
            {
                targetFXList = activeSteamFlowFX;
            }

            if (targetFXList != null && targetFXList.Count > 0)
            {
                int randomFXIndex = Random.Range(0, targetFXList.Count);
                targetFXList[randomFXIndex].transform.position = newPosition;
                targetFXList[randomFXIndex].transform.rotation = newRotation;
            }
            else
            {
                Debug.LogError("No active particle effects to move.");
            }
        }

        //checks the current built location if there is any permenant particle effects and moves them to the new position according to input.
        public void CheckForParticlesAtPosition(ParticleType particleType, Vector3 position, Vector3 newPosition)
        {
            if(particleType == ParticleType.BloodFlow)
            {
                foreach(var bloodFX in activeBloodFlowFX)
                {
                    if (bloodFX.transform.position == position)
                    {
                        bloodFX.transform.position = newPosition;
                    }
                }
            }
            else if (particleType == ParticleType.SteamFlow)
            {
                foreach (var steamFX in activeSteamFlowFX)
                {
                    if (steamFX.transform.position == position)
                    {
                        steamFX.transform.position = newPosition;
                    }
                }
            }
            else
            {
                Debug.LogError("Particle type is meant to persist!");
            }
        }
    }
}