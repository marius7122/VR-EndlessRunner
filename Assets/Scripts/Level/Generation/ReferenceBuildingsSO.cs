using UnityEngine;

namespace Level.Generation
{
    [CreateAssetMenu(fileName = "ReferenceBuildings", menuName = "ScriptableObjects/Level/ReferenceBuildings")]
    public class ReferenceBuildingsSO : ScriptableObject
    {
        public ReferenceBuilding[] prefabs;

        private int _lastPick = -1;
        
        public ReferenceBuilding PickRandom()
        {
            var randomIndex = Random.Range(0, prefabs.Length);
            if (randomIndex == _lastPick && prefabs.Length > 0)
            {
                if (randomIndex + 1 < prefabs.Length)
                    randomIndex++;
                else
                    randomIndex--;
            }

            _lastPick = randomIndex;
            return prefabs[randomIndex];
        }
    }
}