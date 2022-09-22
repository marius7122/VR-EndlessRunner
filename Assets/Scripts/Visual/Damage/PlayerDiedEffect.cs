using DG.Tweening;
using Events.ScriptableObjects;
using Unity.Rendering.HybridV2;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Visual.Damage
{
    public class PlayerDiedEffect : MonoBehaviour
    {
        [SerializeField] private VoidEventChannelSO onPlayerDied;
        
        [SerializeField] private SpriteRenderer fovBlocker;
        [SerializeField] private Color damageColor;
        [SerializeField] private Color afterDamageColor;

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
            var sequence = DOTween.Sequence();

            sequence.Append(fovBlocker.DOColor(damageColor, 2f));
            sequence.Append(fovBlocker.DOColor(afterDamageColor, 1.5f));
            sequence.AppendInterval(1f);
            sequence.AppendCallback(() =>
            {
                SceneManager.LoadScene(0);
            });
        }
    }
}