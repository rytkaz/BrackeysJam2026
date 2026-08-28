using System;
using System.Text;
using BlocksGame.Gameplay;
using R3;
using TMPro;
using UnityEngine;

namespace BlocksGame.UI
{
    public class GameUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI scoreText;
        [SerializeField] private TextMeshProUGUI timeText;
        [SerializeField] private GameplayController gameplayController;

        private IDisposable disposable;

        private void Start()
        {
            var builder = Disposable.CreateBuilder();
            gameplayController.SecondsElapsed.ObserveOnMainThread().Subscribe(UpdateTimeElapsed).AddTo(ref builder);
            gameplayController.Score.ObserveOnMainThread().Subscribe(score => { scoreText.SetText(score.ToString()); }).AddTo(ref builder);
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
    }
}

