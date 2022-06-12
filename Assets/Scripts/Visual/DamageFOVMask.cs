using System;
using Events.ScriptableObjects;
using UnityEngine;

namespace Visuals
{
    public class DamageFOVMask : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer fovMask;
        [SerializeField] private VoidEventChannelSO playerWasHit;

        private void OnEnable()
        {
            playerWasHit.OnEventRaised += DisplayDamageFOVMask;
        }

        private void OnDisable()
        {
            playerWasHit.OnEventRaised -= DisplayDamageFOVMask;
        }

        private void DisplayDamageFOVMask()
        {
            fovMask.enabled = true;
        }
    }
}