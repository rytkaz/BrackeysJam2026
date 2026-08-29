using MessagePipe;
using UnityEngine.SceneManagement;

namespace BlocksGame.UI
{
    public class PauseScreen : GameUIScreen
    {
        protected override void Show()
        {
            gameObject.SetActive(true);
            GlobalMessagePipe.GetPublisher<MGamePauseStateChanged>().Publish(new MGamePauseStateChanged() { IsPaused = true });
        }

        protected override void Hide()
        {
            gameObject.SetActive(false);
            GlobalMessagePipe.GetPublisher<MGamePauseStateChanged>().Publish(new MGamePauseStateChanged() { IsPaused = false });
        }

        public void ResumeClicked()
        {
            Hide();
        }

        public void MainMenuClicked()
        {
            SceneManager.LoadScene(SceneNames.Menu);
        }
    }
}
