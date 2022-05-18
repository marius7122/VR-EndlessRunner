using System;
using UnityEngine;

namespace Locomotion
{
    public class FovRestrictorLogic : MonoBehaviour
    {
        [SerializeField] private FovRestrictor restrictor;

        [SerializeField] private float minFov = 90f;
        [SerializeField] private float maxFov = 170f;
        [SerializeField] private float impactSpeedForMinFov = 5f;
            
        private float _targetTime;
        private float _targetFov;
        private float _fovChangingRate = 5f;
        
        private void Awake()
        {
            restrictor = restrictor ? restrictor : GetComponent<FovRestrictor>();
            if (!restrictor)
            {
                Debug.LogWarning($"[{nameof(FovRestrictorLogic)}] FovRestrictor not assigned");
            }
        }

        private void Update()
        {
            UpdateFov();
        }

        public void RestrictFovBecauseImpact(float impactSpeed)
        {
            float impactTargetFov = maxFov - (impactSpeed / impactSpeedForMinFov) * (maxFov - minFov);
            impactTargetFov = Mathf.Clamp(impactTargetFov, minFov, maxFov);

            // if (restrictor.CurrentFOV < impactTargetFov || (_targetTime < Time.time && _targetFov < impactTargetFov))
            //     return;


            // SetToFOVInTime(impactTargetFov, 0.15f);
            restrictor.RestrictFov(impactTargetFov);
        }

        private void UpdateFov()
        {
            float newFov = restrictor.CurrentFOV + _fovChangingRate * Time.deltaTime;
            newFov = Mathf.Clamp(newFov, minFov, maxFov);
            restrictor.RestrictFov(newFov);
        }
        
        private void SetToFOVInTime(float fov, float time)
        {
            _targetTime = Time.time + time;
            _targetFov = fov;
            _fovChangingRate = (fov - restrictor.CurrentFOV) / time;
            restrictor.RestrictFov(fov);
        }
    }
}