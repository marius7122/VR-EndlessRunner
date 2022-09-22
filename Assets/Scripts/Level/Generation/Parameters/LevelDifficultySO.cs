using UnityEngine;

namespace Level.Generation.Parameters
{
    [CreateAssetMenu(fileName = "LevelDifficulty", menuName = "ScriptableObjects/Level/LevelDifficulty")]
    public class LevelDifficultySO : ScriptableObject
    {
        [Range(0f, 1f)] public float jumpDifficultyFactor = 0.8f;
        public float projectileSpawnerChance = 0.1f;
    }
}