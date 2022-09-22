using Common;
using Events.ScriptableObjects;
using UnityEngine;

namespace Enemies.Environment
{
    public class DieBecauseImpact : MonoBehaviour
    {
        [SerializeField] private VoidEventChannelSO playerDiedChannel;
        [SerializeField] private CharacterCollisionDetector collisionDetector;
        [SerializeField] private float maximumTolerableImpactSpeed = 10f;

        private void OnEnable()
        {
            collisionDetector.OnImpact += AnalyzeImpactSeverity;
        }

        private void OnDisable()
        {
            collisionDetector.OnImpact -= AnalyzeImpactSeverity;
        }

        private void AnalyzeImpactSeverity(Vector3 impactSpeed)
        {
            Debug.Log($"Impact speed: {impactSpeed.magnitude}");

            if (impactSpeed.magnitude > maximumTolerableImpactSpeed)
            {
                playerDiedChannel.RaiseEvent();
            }
        }
    }
}
