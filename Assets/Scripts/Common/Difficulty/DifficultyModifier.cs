using UnityEngine;

namespace Common.Difficulty
{
    public class DifficultyModifier : MonoBehaviour
    {
        private const string DifficultyKey = "GameDifficulty";
        
        public void SetDifficulty(DifficultyType difficulty)
        {
            PlayerPrefs.SetInt(DifficultyKey, (int)difficulty);
        }

        public void SetEasyDifficulty() => SetDifficulty(DifficultyType.Easy);
        public void SetMediumDifficulty() => SetDifficulty(DifficultyType.Medium);
        public void SetHardDifficulty() => SetDifficulty(DifficultyType.Hard);

        public DifficultyType GetDifficulty()
        {
            return (DifficultyType)PlayerPrefs.GetInt(DifficultyKey);
        }
    }
}