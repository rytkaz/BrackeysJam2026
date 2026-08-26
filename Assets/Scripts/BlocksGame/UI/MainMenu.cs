using UnityEngine;
using UnityEngine.SceneManagement;

namespace BlocksGame.UI
{
    public class MainMenu : MonoBehaviour
    {
        public void Play()
        {
            SceneManager.LoadScene(Constants.GameScene);
        }
        
        public void Quit()
        {
            Application.Quit();
        }
    }
}
