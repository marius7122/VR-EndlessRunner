using Locomotion.Utils;
using UnityEngine;

namespace Debugging
{
    public class XRControllerSpeedVisualiser : MonoBehaviour
    {
        [SerializeField] private float scaleFactor = 0.2f;
        [SerializeField] private XRControllerMovementInfoBehavior referencedController;

        private void Update()
        {
            var controllerSpeed = referencedController.Speed;

            transform.LookAt(transform.position + controllerSpeed);
            var scale = transform.localScale;
            scale.z = controllerSpeed.magnitude * scaleFactor;
            transform.localScale = scale;
        }
    }
}
