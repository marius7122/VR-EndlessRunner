using UnityEngine;

namespace Debugging
{
    public class ConstantSpeed : MonoBehaviour
    {
        [SerializeField] private Vector3 speed;

        private void Update()
        {
            transform.position += speed * Time.deltaTime;
        }
    }
}