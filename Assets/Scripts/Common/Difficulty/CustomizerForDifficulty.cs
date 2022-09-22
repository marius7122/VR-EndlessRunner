using UnityEngine;
using UnityEngine.Events;

namespace Common.Difficulty
{
    public class CustomizerForDifficulty : MonoBehaviour
    {
        [SerializeField] private DifficultyModifier difficulty;
        
        public UnityEvent onEasy;
        public UnityEvent onMedium;
        public UnityEvent onHard;

        private void Awake()
        {
            switch (difficulty.GetDifficulty())
            {
                case DifficultyType.Easy: 
                    onEasy.Invoke();
                    break;
                case DifficultyType.Medium: 
                    onMedium.Invoke();
                    break;
                case DifficultyType.Hard: 
                    onHard.Invoke();
                    break;
            }
        }
    }
}