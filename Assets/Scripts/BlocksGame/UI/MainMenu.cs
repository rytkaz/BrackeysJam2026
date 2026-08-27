using UnityEngine;
using UnityEngine.SceneManagement;

namespace BlocksGame.UI
{
    public class MainMenu : MonoBehaviour
    {
        public void Play()
        {
            SceneManager.LoadScene(SceneNames.Game);
        }
        
        public void Quit()
        {
            Application.Quit();
        }
    }
}
