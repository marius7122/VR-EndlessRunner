using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace Debugging
{
    public class RestartLevelOnInput : MonoBehaviour
    {
        [SerializeField] private InputActionReference actionForRestart;


        private void OnEnable()
        {
            actionForRestart.action.performed += Restart;
        }

        private void OnDisable()
        {
            actionForRestart.action.performed += Restart;
        }

        private void Restart(InputAction.CallbackContext ctx)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
