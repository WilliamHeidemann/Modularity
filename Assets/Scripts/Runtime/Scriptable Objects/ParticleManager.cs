using System;
using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using Runtime.DataLayer;
using Codice.CM.Client.Differences.Merge;
using Random = UnityEngine.Random;
using System.Collections;

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

        //this method needs to be called at random interavals to move the particle effects around the map, this should be done in another script through update.
        public void MoveRandomParticleFX()
        {
            List<GameObject> targetFXList = null;
            Vector3[] connectionTransform = new Vector3[2];
            float randomNumber = Random.Range(0f, 1f);

            if (randomNumber < 0.5f)
            {
                targetFXList = activeBloodFlowFX;
                connectionTransform = GetOpenConnectionTransform(ParticleType.BloodFlow);
            }
            else
            {
                targetFXList = activeSteamFlowFX;
                connectionTransform = GetOpenConnectionTransform(ParticleType.SteamFlow);
            }

            if (targetFXList != null && targetFXList.Count > 0)
            {
                int randomFXIndex = Random.Range(0, targetFXList.Count);
                targetFXList[randomFXIndex].transform.position = connectionTransform[0];
                targetFXList[randomFXIndex].transform.rotation = Quaternion.Euler(connectionTransform[1]);
            }
            else
            {
                Debug.LogError("No active particle effects to move.");
            }
        }

        public void MoveSpecificParticleFX(GameObject particleToMove, ParticleType particleType)
        {
            List<GameObject> targetFXList = null;
            Vector3[] connectionTransform = new Vector3[2];

            if (particleType == ParticleType.BloodFlow)
            {
                targetFXList = activeBloodFlowFX;
                connectionTransform = GetOpenConnectionTransform(ParticleType.BloodFlow);
            }
            else
            {
                targetFXList = activeSteamFlowFX;
                connectionTransform = GetOpenConnectionTransform(ParticleType.SteamFlow);
            }

            if (targetFXList != null && targetFXList.Count > 0)
            {
                particleToMove.transform.position = connectionTransform[0];
                particleToMove.transform.rotation = Quaternion.Euler(connectionTransform[1]);
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
                        MoveSpecificParticleFX(bloodFX, ParticleType.BloodFlow);
                    }
                }
            }
            else if (particleType == ParticleType.SteamFlow)
            {
                foreach (var steamFX in activeSteamFlowFX)
                {
                    if (steamFX.transform.position == position)
                    {
                        MoveSpecificParticleFX(steamFX, ParticleType.SteamFlow);
                    }
                }
            }
            else
            {
                Debug.LogError("Particle type is meant to persist!");
            }
        }

        //this is meant solely as an exsample, this method should be made elsewhere and linked up, then delete what is inbetween these lines.
        //=======================================================================================================
        public Vector3[] GetOpenConnectionTransform(ParticleType particleType)
        {
            Vector3[] connectionTransform = new Vector3[2];
            if (particleType == ParticleType.BloodFlow)
            {
                connectionTransform[0] = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));
                connectionTransform[1] = Quaternion.Euler(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360)).eulerAngles;
            }
            else if (particleType == ParticleType.SteamFlow)
            {
                connectionTransform[0] = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));
                connectionTransform[1] = Quaternion.Euler(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360)).eulerAngles;
            }
            else
            {
                Debug.LogError("Particle type is not meant to be a permanent type!");
            }
            return connectionTransform;
        }

        //=======================================================================================================
    }
}