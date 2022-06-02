using System;
using UnityEngine;

namespace Utils
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