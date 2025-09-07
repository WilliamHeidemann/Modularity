using LiteDB;
using Runtime.DataLayer;
using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Runtime.Scriptable_Objects
{
    [CreateAssetMenu]
    public class ParticleManager : ScriptableObject
    {
        [Header("Permanent VFX Behaviour")]
        public bool permanentParticles = true;
        [SerializeField] private int _particleGrowthRate = 5; //how many openings are needed before a new particle spawns.
        [SerializeField] private int _particleLimit = 20; //max number of particles that can be active at once.

        [Header("Inputs")]
        [SerializeField] private Structure _structure;
        [SerializeField] private GameObject _bloodFlowFX;
        [SerializeField] private GameObject _steamFlowFX;
        [SerializeField] private GameObject _fleshBurstFX;
        [SerializeField] private GameObject _gearBurstFX;
        [SerializeField] private GameObject _hybridBurstFX;

        private List<GameObject> activeBloodFlowFX = new List<GameObject>();
        private List<GameObject> activeSteamFlowFX = new List<GameObject>();

        private Dictionary<Vector3, Quaternion> fleshSlots = new Dictionary<Vector3, Quaternion>();
        private Dictionary<Vector3, Quaternion> metalSlots = new Dictionary<Vector3, Quaternion>();

        //clear both lists must be called at the start of the game.
        public void Initialize()
        {
            activeBloodFlowFX.Clear();
            activeSteamFlowFX.Clear();
            UpdateParticleSlots();
        }

        public void UpdateParticleSlots()
        {
            fleshSlots.Clear();
            metalSlots.Clear();

            var positionAndRotations = _structure.GetOpenSlots(DataLayer.ConnectionType.Blood);
            foreach (var (position, rotation) in positionAndRotations)
            {
                fleshSlots.Add(position, rotation);
            }
            positionAndRotations = _structure.GetOpenSlots(DataLayer.ConnectionType.Steam);
            foreach (var (position, rotation) in positionAndRotations)
            {
                metalSlots.Add(position, rotation);
            }

            CheckForParticleSlotClosed();
            UpdateParticleAmount();
        }

        private void DestroyAllPermenantParticles()
        {
            foreach (var bloodFX in activeBloodFlowFX)
            {
                if (bloodFX != null)
                {
                    Destroy(bloodFX);
                }
            }
            foreach (var steamFX in activeSteamFlowFX)
            {
                if (steamFX != null)
                {
                    Destroy(steamFX);
                }
            }
            activeBloodFlowFX.Clear();
            activeSteamFlowFX.Clear();
        }

        private void UpdateParticleAmount()
        {
            if(activeBloodFlowFX.Count + activeSteamFlowFX.Count >= _particleLimit)
            {
                return;
            }

            if(fleshSlots.Count >= (activeBloodFlowFX.Count + 1) * _particleGrowthRate)
            {
                Vector3? newSlot = GetRandomAvailableSlot(fleshSlots);
                SpawnParticleFX(ParticleType.BloodFlow, newSlot.Value, fleshSlots[newSlot.Value], true);
            }
            if(metalSlots.Count >= (activeSteamFlowFX.Count + 1) * _particleGrowthRate)
            {
                Vector3? newSlot = GetRandomAvailableSlot(metalSlots);
                SpawnParticleFX(ParticleType.SteamFlow, newSlot.Value, metalSlots[newSlot.Value], true);
            }
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
            else if(permanentParticles)
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

        public Vector3? GetRandomAvailableSlot(Dictionary<Vector3, Quaternion> availableSlots)
        {
            const int maxAttempts = 20;
            int attempt = 0;
            var slotList = new List<KeyValuePair<Vector3, Quaternion>>(availableSlots);

            if (slotList.Count == 0)
                return null;

            while (attempt < maxAttempts)
            {
                var randomIndex = Random.Range(0, slotList.Count);
                var randomEntry = slotList[randomIndex];

                // Check if the slot is already occupied by another VFX
                if (!IsParticleAtSlot(randomEntry.Key))
                {
                    return randomEntry.Key;
                }

                Debug.Log("Attempt " + attempt + " failed to find an unoccupied slot.");
                attempt++;
            }

            // No available slot found after max attempts
            return null;
        }

        public void MoveSpecificParticleFX(GameObject particleToMove, Dictionary<Vector3, Quaternion> availableSlots)
        {
            Vector3? newSlot = GetRandomAvailableSlot(availableSlots);

            if (newSlot.HasValue)
            {
                particleToMove.transform.position = newSlot.Value;
                particleToMove.transform.rotation = availableSlots[newSlot.Value];
                return;
            }

            // Destroys the particle if it cannot find a new slot after max attempts so there wouldn't be two particles in one spot.
            if (activeBloodFlowFX.Contains(particleToMove))
            {
                activeBloodFlowFX.Remove(particleToMove);
            }
            else if (activeSteamFlowFX.Contains(particleToMove))
            {
                activeSteamFlowFX.Remove(particleToMove);
            }
            Destroy(particleToMove);
        }

        public bool IsParticleAtSlot(Vector3 slotPosition)
        {
            foreach (GameObject VFX in activeBloodFlowFX)
            {
                if (VFX.transform.position == slotPosition)
                {
                    return true;
                }
            }
            foreach (GameObject VFX in activeSteamFlowFX)
            {
                if (VFX.transform.position == slotPosition)
                {
                    return true;
                }
            }
            return false;
        }

        public void CheckForParticleSlotClosed()
        {
            if(!permanentParticles)
            {
                return;
            }

            //checks through the flesh slots positions and if any of the active blood flow particles are not at a valid position, move them to a new random position.
            if (activeBloodFlowFX != null)
            {
                foreach (var bloodFX in activeBloodFlowFX)
                {
                    if (bloodFX == null) continue;

                    if (!fleshSlots.ContainsKey(bloodFX.transform.position))
                    {
                        MoveSpecificParticleFX(bloodFX, fleshSlots);
                    }
                }
            }
            //checks through the metal slots positions and if any of the active steam flow particles are not at a valid position, move them to a new random position.
            if (activeSteamFlowFX != null)
            {
                foreach (var steamFX in activeSteamFlowFX)
                {
                    if(steamFX == null) continue;

                    if (!metalSlots.ContainsKey(steamFX.transform.position))
                    {
                        MoveSpecificParticleFX(steamFX, metalSlots);
                    }
                }
            }
        }
    }
}