using UnityEngine;

namespace BlocksGame.UI
{
    public abstract class GameUIScreen : MonoBehaviour
    {
        [SerializeField] private bool hiddenByDefault = true;

        private bool isShown = false;
        
        protected virtual void Awake()
        {
            if (hiddenByDefault)
            {
                Hide();
            }
        }

        public void Toggle()
        {
            if (isShown)
            {
                isShown = false;
                Hide();
            }
            else
            {
                isShown = true;
                Show();
            }
        }

        protected abstract void Show();
        protected abstract void Hide();
    }
}
