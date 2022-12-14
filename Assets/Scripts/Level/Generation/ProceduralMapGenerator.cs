using System.Collections.Generic;
using Enemies.Projectile;
using Level.Generation.Buildings;
using Level.Generation.Parameters;
using UnityEngine;

namespace Level.Generation
{
    public class ProceduralMapGenerator : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField] private Building buildingPrefab;
        [SerializeField] private ProjectileSpawner projectileSpawnerPrefab;
        [SerializeField] private Building startBuilding;
        [SerializeField] private Transform player;
        [SerializeField] private PlayerMovementParameters movement;
        [SerializeField] private LevelDifficultySO levelDifficulty;
        [SerializeField] private ProjectilePropertiesSO projectileDifficulty;
        
        [Header("Customization")]
        [SerializeField] private float distanceToBuildAhead = 250f;
        [SerializeField] private float distanceToDestroy = 100f;
        [SerializeField] private float minLength = 10f;
        [SerializeField] private float maxLength = 20f;
        [SerializeField] private float width = 6f;
        [SerializeField] private float minGap = 1.5f;
        

        private LinkedList<Building> _instantiatedBuildings;
        private LinkedList<ProjectileSpawner> _instantiatedSpawners;
        private ProjectileTarget _projectileTarget;

        private void Awake()
        {
            _instantiatedBuildings = new LinkedList<Building>();
            _instantiatedBuildings.AddFirst(startBuilding);
            _instantiatedSpawners = new LinkedList<ProjectileSpawner>();

            _projectileTarget = player.GetComponentInChildren<ProjectileTarget>();
            if (_projectileTarget == null)
            {
                Debug.LogWarning("Player don't have a ProjectileTarget component attached => projectiles spawners won't work");
            }
        }

        private void Update()
        {
            UpdateBuildings();
        }

        public void SetLevelDifficulty(LevelDifficultySO difficulty)
        {
            levelDifficulty = difficulty;
        }

        public void SetProjectileDifficulty(ProjectilePropertiesSO difficulty)
        {
            projectileDifficulty = difficulty;
        }
        

        private void UpdateBuildings()
        {
            AppendBuildings();
            DestroyOutOfScopeBuildings();
            DestroyOutOfScopeProjectileSpawners();
        }

        private void AppendBuildings()
        {
            var lastBuilding = _instantiatedBuildings.Last.Value;
            var minimumCoveredZ = player.transform.position.z + distanceToBuildAhead;

            while (lastBuilding.FinishZPosition < minimumCoveredZ)
            {
                lastBuilding = InstantiateRandomBuildingAfter(lastBuilding);
                _instantiatedBuildings.AddLast(lastBuilding);

                if (_projectileTarget != null && Random.value <= levelDifficulty.projectileSpawnerChance)
                {
                    var newSpawner = InstantiateProjectileSpawnerAbove(lastBuilding);
                    _instantiatedSpawners.AddLast(newSpawner);
                }
            }
        }

        private Building InstantiateRandomBuildingAfter(Building lastBuilding)
        {
            var length = Random.Range(minLength, maxLength);

            var gap = Random.Range(minGap, movement.MaxJumpDistance() * levelDifficulty.jumpDifficultyFactor);

            var maxHeightDifference = movement.MaxHeightDifference(gap);
            var heightDifference = Random.Range(-maxHeightDifference, maxHeightDifference * levelDifficulty.jumpDifficultyFactor);
            var height = lastBuilding.Size.y + heightDifference;

            var newBuilding = Instantiate(buildingPrefab, transform);
            newBuilding.Size = new Vector3(width, height, length);
            newBuilding.StartingZPosition = lastBuilding.FinishZPosition + gap;
            
            // Debug.Log($"Gap: {gap} => maxHDifference = {maxHeightDifference} => HDifference = {heightDifference}");
            
            return newBuilding;
        }

        private ProjectileSpawner InstantiateProjectileSpawnerAbove(Building building)
        {
            var newProjectileSpawner = Instantiate(projectileSpawnerPrefab, building.transform, true);
            newProjectileSpawner.SetTarget(_projectileTarget);
            newProjectileSpawner.SetProjectileProperties(projectileDifficulty);

            var buildingMidAndAbove = building.transform.position +
                              Vector3.up * (building.Size.y + 20f) +
                              Vector3.forward * building.Size.z * 0.5f;

            var spawnerPosition = buildingMidAndAbove;
            switch (Random.Range(0, 3))
            {
                case 0:     // middle
                    break;
                case 1:     // left
                    spawnerPosition += Vector3.left * building.Size.x * 0.5f;
                    break;
                case 2:     // right
                    spawnerPosition += Vector3.right * building.Size.x * 0.5f;
                    break;
            }
            
            newProjectileSpawner.transform.position = spawnerPosition;

            return newProjectileSpawner;
        }

        private void DestroyOutOfScopeBuildings()
        {
            var firstBuilding = _instantiatedBuildings.First.Value;
            var maximumCoveredZ = player.transform.position.z - distanceToDestroy;

            while (firstBuilding.FinishZPosition < maximumCoveredZ)
            {
                Destroy(firstBuilding.gameObject);
                _instantiatedBuildings.RemoveFirst();
                firstBuilding = _instantiatedBuildings.First.Value;
            }
        }

        private void DestroyOutOfScopeProjectileSpawners()
        {
            if (_instantiatedSpawners.Count == 0)
                return;

            var firstSpawner = _instantiatedSpawners.First.Value;
            while (firstSpawner.transform.position.z < player.position.z)
            {
                _instantiatedSpawners.RemoveFirst();
                Destroy(firstSpawner.gameObject);

                if (_instantiatedSpawners.Count == 0)
                    return;
                firstSpawner = _instantiatedSpawners.First.Value;
            }
        }
    }
}


