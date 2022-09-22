using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace Debugging
{
    [RequireComponent(typeof(XRGrabInteractable))]
    public class XRInteractableChangeColorOnEvent : MonoBehaviour
    {
        [SerializeField] private Color selectEnterColor = Color.blue;
        [SerializeField] private Color selectExitColor = Color.magenta;
    
        private XRGrabInteractable _interactable;
        private MeshRenderer _renderer;

        private void Awake()
        {
            _interactable = GetComponent<XRGrabInteractable>();
            _renderer = GetComponent<MeshRenderer>();
        }
        private void OnEnable()
        {
            _interactable.selectEntered.AddListener(OnSelectEnter);
            _interactable.selectExited.AddListener(OnSelectExit);
        }
        private void OnDisable()
        {
            _interactable.selectEntered.RemoveListener(OnSelectEnter);
            _interactable.selectExited.RemoveListener(OnSelectExit);
        }

    
        private void OnSelectEnter(SelectEnterEventArgs args)
        {
            Debug.Log("Selector enter");
            SetColor(selectEnterColor);
        }

        private void OnSelectExit(SelectExitEventArgs args)
        {
            Debug.Log("Selector exit");
            SetColor(selectExitColor);
        }
    
        private void SetColor(Color newColor)
        {
            _renderer.material.color = newColor;
        }
    }
}
