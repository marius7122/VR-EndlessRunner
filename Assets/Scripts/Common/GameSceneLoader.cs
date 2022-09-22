using UnityEngine;
using UnityEngine.SceneManagement;

namespace Common
{
    public class GameSceneLoader : MonoBehaviour
    {
        public void Load()
        {
            SceneManager.LoadScene(1);
        }
    }
}