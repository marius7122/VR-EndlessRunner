using DG.Tweening;
using Events.ScriptableObjects;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Visual.Damage
{
    public class PlayerDiedEffect : MonoBehaviour
    {
        [SerializeField] private VoidEventChannelSO onPlayerDied;
        [SerializeField] private SpriteRenderer redScreen;
            
        private void OnEnable()
        {
            onPlayerDied.OnEventRaised += PlayerDied;
        }
        private void OnDisable()
        {
            onPlayerDied.OnEventRaised -= PlayerDied;
        }

        [ContextMenu("PlayerDied")]
        private void PlayerDied()
        {
            Debug.Log("Player died");
            redScreen.DOFade(1f, 2f).OnComplete(
                () => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex)
            );
        }
    }
}