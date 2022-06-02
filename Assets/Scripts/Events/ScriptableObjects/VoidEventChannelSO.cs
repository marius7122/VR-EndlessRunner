using System;
using UnityEngine;

namespace Events.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Events/Void Event")]
    public class VoidEventChannelSO : ScriptableObject
    {
        public event Action OnEventRaised;

        public void RaiseEvent()
        {
            OnEventRaised?.Invoke();
        }
    }
}
