using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

namespace Visual.UI
{
    [RequireComponent(typeof(XRBaseInteractable))]
    public class CubeButton : MonoBehaviour
    {
        [SerializeField] private MeshRenderer meshRenderer;

        [SerializeField] private Color normalColor;
        [SerializeField] private Color hoveredColor;
        [SerializeField] private Color selectedColor;

        public UnityEvent onSelected;

        private XRBaseInteractable _interactable;

        private void Awake()
        {
            _interactable = GetComponent<XRBaseInteractable>();
        }

        private void OnEnable()
        {
            _interactable.hoverEntered.AddListener(OnHoverEnter);
            _interactable.hoverExited.AddListener(OnHoverExit);
            _interactable.selectEntered.AddListener(OnSelectEnter);
            _interactable.selectExited.AddListener(OnSelectExit);
        }
        
        private void OnDisable()
        {
            _interactable.hoverEntered.RemoveListener(OnHoverEnter);
            _interactable.hoverExited.RemoveListener(OnHoverExit);
            _interactable.selectEntered.RemoveListener(OnSelectEnter);
            _interactable.selectExited.RemoveListener(OnSelectExit);
        }

        private void OnHoverEnter(HoverEnterEventArgs args)
        {
            // Debug.Log("Hover enter");
            
            meshRenderer.material.color = hoveredColor;
            transform.DOScale(Vector3.one * 1.2f, 0.3f).SetEase(Ease.OutBack);

        }

        private void OnHoverExit(HoverExitEventArgs args)
        {
            // Debug.Log("Hover exit");
            
            meshRenderer.material.color = normalColor;
            transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.InBack);
        }

        private void OnSelectEnter(SelectEnterEventArgs args)
        {
            // Debug.Log("Select enter");

            meshRenderer.material.color = selectedColor;
        }

        private void OnSelectExit(SelectExitEventArgs args)
        {
            // Debug.Log("Select exit");

            meshRenderer.material.color = normalColor;
            onSelected.Invoke();
        }
    }
}
