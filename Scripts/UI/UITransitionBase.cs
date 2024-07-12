using System;
using System.Threading.Tasks;
using UnityEngine;

namespace IT.CoreLib.UI
{

    public abstract class UITransitionBase : MonoBehaviour
    {
        public enum TransitionState
        {
            None,
            TransitionIn,
            Hold,
            HoldAndWait,
            TransitionOut
        }

        public event Action<UITransitionBase> OnTransitionStarted;
        public event Action<UITransitionBase> OnTransitionInCompleted;
        public event Action<UITransitionBase> OnTransitionFinishingStarted;
        public event Action<UITransitionBase> OnTransitionCompleted;
        
        public TransitionState CurrentState => _transitionState;
        protected TransitionState _transitionState = TransitionState.None;

        [SerializeField] private float _defaultTransitionTime = 0.5f;

        protected float _transitionInTime;
        protected float _transitionHoldTime;
        protected float _transitionOutTime;
        protected bool _transitionAutoEnd;

        protected float _timer;

        private Task _awaitTask;
        private TaskCompletionSource<bool> _awaitCompletionSource;


        #region PUBLIC

        /// <summary>
        /// Start transition
        /// Task completes when transition comes to hold state
        /// </summary>
        public virtual async Awaitable StartTransitionAsync(
            bool transitionAutoComplete = true,
            float transitionInTime = -1.0f, 
            float transitionOutTime = -1.0f, 
            float transitionHoldTime = 0.0f)
        {
            if (_awaitCompletionSource != null)
                throw new Exception("[UI] Transition can't start: it's already started!");
           
            StartTransition(transitionAutoComplete, transitionInTime, transitionOutTime, transitionHoldTime);

            _awaitCompletionSource = new TaskCompletionSource<bool>();
            await _awaitCompletionSource.Task;
        }

        /// <summary>
        /// Start transition
        /// </summary>
        public virtual void StartTransition(
            bool transitionAutoComplete = true,
            float transitionInTime = -1.0f,
            float transitionOutTime = -1.0f,
            float transitionHoldTime = 0.0f)
        {
            if (_transitionState != TransitionState.None)
                throw new Exception("[UI] Transition can't start: it's already started!");

            _transitionInTime = transitionInTime >= 0.0f ? transitionInTime : _defaultTransitionTime;
            _transitionOutTime = transitionOutTime >= 0.0f ? transitionOutTime : _defaultTransitionTime;
            _transitionAutoEnd = transitionAutoComplete;
            _transitionHoldTime = transitionHoldTime;

            _transitionState = TransitionState.TransitionIn;
            _timer = 0.0f;

            OnTransitionStarted?.Invoke(this);
        }

        /// <summary>
        /// Start transition out
        /// Task completes when transition is completed
        /// </summary>
        public virtual async Awaitable FinishTransitionAsync()
        {
            FinishTransition();
            
            _awaitCompletionSource = new TaskCompletionSource<bool>();
            await _awaitCompletionSource.Task;
        }

        /// <summary>
        /// Start transition out
        /// </summary>
        public virtual void FinishTransition()
        {
            if (_transitionState == TransitionState.None)
                throw new Exception("[UI] Transition can't finish: it's not started or already finished!");

            if (_transitionState == TransitionState.TransitionOut)
                throw new Exception("[UI] Transition can't finish: it's already finishing!");

            if (_awaitCompletionSource != null)
                _awaitCompletionSource.SetResult(true);

            _awaitCompletionSource = null;

            _transitionState = TransitionState.TransitionOut;
            _timer = 0.0f;

            OnTransitionFinishingStarted?.Invoke(this);
        }

        /// <summary>
        /// Finish transitions
        /// Can be called for forced completion
        /// </summary>
        public virtual void CompleteTransition()
        {
            if (_awaitCompletionSource != null)
                _awaitCompletionSource.SetResult(true);

            _awaitCompletionSource = null;

            _timer = 0.0f;
            _transitionState = TransitionState.None;

            ClearAfterTransition();

            OnTransitionCompleted?.Invoke(this);
        }

        #endregion


        #region INTERNAL

        protected virtual void StartHolding()
        {
            if (_transitionState != TransitionState.TransitionIn)
                throw new Exception("[UI] Transition can't hold: it's in a wrong state!");

            if (_awaitCompletionSource != null)
                _awaitCompletionSource.SetResult(true);

            _awaitCompletionSource = null;

            _timer = 0.0f;
            _transitionState = TransitionState.Hold;
        }

        protected virtual void StartHoldAndWait()
        {
            if (_transitionState != TransitionState.Hold)
                throw new Exception("[UI] Transition can't hold and wait: it's in a wrong state!");

            _timer = 0.0f;
            _transitionState = TransitionState.HoldAndWait;
        }

        private void Update()
        {
            if (CurrentState == TransitionState.None || CurrentState == TransitionState.HoldAndWait)
                return;

            _timer += Time.unscaledDeltaTime;

            if (CurrentState == TransitionState.TransitionIn)
            {
                UpdateTransitionIn(Time.unscaledDeltaTime);

                if (_timer >= _transitionInTime)
                {
                    OnTransitionInCompleted?.Invoke(this);
                    StartHolding();

                    return;
                }
            }

            if (CurrentState == TransitionState.TransitionOut)
            {
                UpdateTransitionOut(Time.unscaledDeltaTime);

                if (_timer >= _transitionOutTime)
                {
                    CompleteTransition();
                    return;
                }
            }

            if (CurrentState == TransitionState.Hold)
            {
                UpdateTransitionHold(Time.unscaledDeltaTime);

                if (_timer >= _transitionHoldTime)
                {
                    if (_transitionAutoEnd)
                        FinishTransition();
                }
            }
        }

        #endregion


        #region TO OVERRIDE

        /// <summary>
        /// Override this for initialization
        /// </summary>
        public abstract void Initialize();

        /// <summary>
        /// Override this to draw transition in
        /// </summary>
        /// <param name="dt">delta time</param>
        protected abstract void UpdateTransitionIn(float dt);

        /// <summary>
        /// Override this to draw transition out
        /// </summary>
        /// <param name="dt">delta time</param>
        protected abstract void UpdateTransitionOut(float dt);

        /// <summary>
        /// Override this to draw transition hold (midpoint)
        /// </summary>
        /// <param name="dt">delta time</param>
        protected abstract void UpdateTransitionHold(float dt);

        /// <summary>
        /// Override this for cleaning after transition
        /// </summary>
        protected abstract void ClearAfterTransition();

        #endregion

    }

}
