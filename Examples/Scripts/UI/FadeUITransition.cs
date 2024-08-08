using IT.CoreLib.UI;
using UnityEngine;

namespace IT.CoreLib.Examples.UI
{

    public class FadeUITransition : UITransitionBase
    {
        [SerializeField] private CanvasGroup _canvasGroup;

        public override void Initialize()
        {
            _canvasGroup.alpha = 0.0f;
        }

        protected override void UpdateTransitionIn(float dt)
        {
            _canvasGroup.alpha = _timer / _transitionInTime;
        }

        protected override void UpdateTransitionHold(float dt)
        {
            _canvasGroup.alpha = 1.0f;
        }       

        protected override void UpdateTransitionOut(float dt)
        {
            _canvasGroup.alpha = 1.0f - _timer / _transitionOutTime;
        }

        protected override void ClearAfterTransition()
        {
            _canvasGroup.alpha = 0.0f;
        }
    }

}
