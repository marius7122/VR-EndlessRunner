using UnityEngine;

namespace Locomotion.Utils
{
    public class XRControllerMovementInfoMock : XRControllerMovementInfoBehavior
    {
        public Vector3 mockSpeed;
        public Vector3 mockAcceleration;
        public Vector3 mockForward;
    
        public override Vector3 Speed => mockSpeed;
        public override Vector3 Acceleration => mockAcceleration;
        public override Vector3 Forward => mockForward;
    }
}
