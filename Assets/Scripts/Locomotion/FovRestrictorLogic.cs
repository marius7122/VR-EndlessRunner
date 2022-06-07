using System;
using Locomotion.Utils;
using UnityEngine;

namespace Locomotion
{
    public class FovRestrictorLogic : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField] private FovRestrictor restrictor;
        [SerializeField] private CharacterController characterController;
        [SerializeField] private CharacterCollisionDetector collisionDetector;


        [Header("Customization")]
        [SerializeField] private float minFov = 80f;
        [SerializeField] private float maxFov = 170f;
        [SerializeField] private float impactSpeedForMinFov = 20f;
        [SerializeField] private float speedToContractionRatioFactor = 2f;
        [SerializeField] private float fovGrowingRate = 5f;
        
        private void Awake()
        {
            restrictor = restrictor ? restrictor : GetComponent<FovRestrictor>();
            if (!restrictor)
            {
                Debug.LogWarning($"[{nameof(FovRestrictorLogic)}] FovRestrictor not assigned");
            }
        }

        private void OnEnable()
        {
            collisionDetector.OnImpact += RestrictFovBecauseImpact;
        }

        private void OnDisable()
        {
            collisionDetector.OnImpact -= RestrictFovBecauseImpact;
        }

        private void Update()
        {
            UpdateFov();
        }

        private void RestrictFovBecauseImpact(Vector3 speedLostOnImpact)
        {
            float impactTargetFov = maxFov - (speedLostOnImpact.magnitude / impactSpeedForMinFov) * (maxFov - minFov);
            impactTargetFov = Mathf.Clamp(impactTargetFov, minFov, maxFov);
            
            if(impactTargetFov < restrictor.CurrentFOV)
                restrictor.RestrictFov(impactTargetFov);
        }

        private void UpdateFov()
        {
            float fovChangeRate;
            var characterSpeed = characterController.velocity;
            if (Mathf.Approximately(characterSpeed.y, 0f))
                fovChangeRate = fovGrowingRate;
            else
                fovChangeRate = -(Mathf.Abs(characterSpeed.y) * speedToContractionRatioFactor);


            float newFov = restrictor.CurrentFOV + fovChangeRate * Time.deltaTime;
            newFov = Mathf.Clamp(newFov, minFov, maxFov);
            restrictor.RestrictFov(newFov);
        }
        
    }
}