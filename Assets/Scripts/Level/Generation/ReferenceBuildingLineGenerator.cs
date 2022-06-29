using System.Collections.Generic;
using UnityEngine;

namespace Level.Generation
{
    public class ReferenceBuildingLineGenerator : MonoBehaviour
    {
        [SerializeField] private ReferenceBuildingsSO referenceBuildingsSO;
        [SerializeField] private Transform player;
        
        [SerializeField] private float gapBetween = 15f;
        [SerializeField] private float distanceToBuildAhead = 500f;
        [SerializeField] private float distanceToDestroy = 100f;

        private LinkedList<ReferenceBuilding> _instantiatedBuildings;

        private void Awake()
        {
            _instantiatedBuildings = new LinkedList<ReferenceBuilding>();
        }

        private void Update()
        {
            UpdateReferenceBuildings();
        }

        private void UpdateReferenceBuildings()
        {
            AppendReferenceBuildings();
            DestroyOutOfScopeReferenceBuildings();
        }

        private void AppendReferenceBuildings()
        {
            if (_instantiatedBuildings.Count == 0)
            {
                var firstBuilding = InstantiateRandomReferenceBuilding();
                firstBuilding.transform.localPosition = Vector3.zero;
                _instantiatedBuildings.AddFirst(firstBuilding);
            }
            
            
            var lastBuilding = _instantiatedBuildings.Last.Value;
            var minimumCoveredZ = player.transform.localPosition.z + distanceToBuildAhead;

            while (lastBuilding.MaxZ < minimumCoveredZ)
            {
                var nextPosition = Vector3.forward * (lastBuilding.MaxZ + gapBetween);
                lastBuilding = InstantiateRandomReferenceBuilding();
                lastBuilding.transform.localPosition = nextPosition;
                
                _instantiatedBuildings.AddLast(lastBuilding);
            }
        }

        private ReferenceBuilding InstantiateRandomReferenceBuilding()
        {
            return Instantiate(referenceBuildingsSO.PickRandom(), transform);
        }
        
        private void DestroyOutOfScopeReferenceBuildings()
        {
            if (_instantiatedBuildings.Count == 0)
                return;
            
            var firstBuilding = _instantiatedBuildings.First.Value;
            var maximumCoveredZ = player.transform.localPosition.z - distanceToDestroy;

            while (firstBuilding.MaxZ < maximumCoveredZ)
            {
                Destroy(firstBuilding.gameObject);
                _instantiatedBuildings.RemoveFirst();
                firstBuilding = _instantiatedBuildings.First.Value;
            }
        }
    }
}