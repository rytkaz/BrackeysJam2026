using UnityEngine;
using UnityEngine.SceneManagement;

namespace BlocksGame.UI
{
    public class GameOverScreen : GameUIScreen
    {
        protected override void Show()
        {
            gameObject.SetActive(true);
        }

        protected override void Hide()
        {
            gameObject.SetActive(false);
        }

        public void PlayAgainClicked()
        {
            SceneManager.LoadScene(SceneNames.Game);
        }

        public void MainMenuClicked()
        {
            SceneManager.LoadScene(SceneNames.Menu);
        }
    }
}
