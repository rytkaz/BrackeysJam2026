using System;
using System.Text;
using BlocksGame.Gameplay;
using MessagePipe;
using R3;
using TMPro;
using UnityEngine;

namespace BlocksGame.UI
{
    public enum GameUIScreenType
    {
        GameOver,
        Pause
    }
    
    public class GameUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI scoreText;
        [SerializeField] private TextMeshProUGUI timeText;
        [SerializeField] private GameplayController gameplayController;
        [SerializeField] private SerializedDictionary<GameUIScreenType, GameUIScreen> screens = new SerializedDictionary<GameUIScreenType, GameUIScreen>();

        private IDisposable disposable;

        private void Start()
        {
            var builder = Disposable.CreateBuilder();
            gameplayController.SecondsElapsed.ObserveOnMainThread().Subscribe(UpdateTimeElapsed).AddTo(ref builder);
            gameplayController.Score.ObserveOnMainThread().Subscribe(score => { scoreText.SetText(score.ToString()); }).AddTo(ref builder);
            GlobalMessagePipe.GetSubscriber<MToggleGameUI>().Subscribe(ToggleScreen).AddTo(ref builder);
            disposable = builder.Build();
        }

        private void UpdateTimeElapsed(int time)
        {
            TimeSpan timeElapsed = TimeSpan.FromSeconds(time);
            var sb = new StringBuilder();
            if (timeElapsed.TotalHours > 1)
            {
                if (timeElapsed.TotalHours < 10)
                {
                    sb.Append("0");
                }
                sb.Append(Math.Floor(timeElapsed.TotalHours));
                sb.Append(":");
            }
            sb.Append(timeElapsed.ToString(@"mm\:ss"));
            timeText.text = sb.ToString();
        }

        private void OnDestroy()
        {
            disposable?.Dispose();
        }

        private void ToggleScreen(MToggleGameUI args)
        {
            if (screens.TryGetValue(args.ScreenType, out var screen))
            {
                screen.Toggle();
            }
        }
    }
}

