using UnityEngine;
using UnityEngine.SceneManagement;

namespace BlocksGame
{
    public class Bootstrap : MonoBehaviour
    {
        private void Awake()
        {
            SceneManager.LoadScene(Constants.MenuScene);
        }
    }
}
