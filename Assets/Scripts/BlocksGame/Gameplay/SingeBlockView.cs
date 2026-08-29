using R3;
using TMPro;
using UnityEngine;

namespace BlocksGame.Gameplay
{
    public class SingeBlockView : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer bg;
        [SerializeField] private GameObject clock;
        [SerializeField] private TextMeshPro clockLabel;
        [SerializeField] private SpriteRenderer enemySprite;
        
        private void Awake()
        {
            clock.gameObject.SetActive(false);
            enemySprite.enabled = false;
        }

        public void SetColor(Color color)
        {
            bg.color = color;
        }
        
        public void ShowClock(Observable<int> timer)
        {
            timer.ObserveOnMainThread().Subscribe((time) =>
            {
                clockLabel.SetText(time.ToString());
            });
            clock.gameObject.SetActive(true);
        }

        public void EnableEnemySprite()
        {
            enemySprite.enabled = true;
        }
    }
}
