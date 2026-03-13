using System;
using System.Collections.Generic;

namespace UniExtension
{
    public abstract class Timer : IDisposable
    {
        public bool IsRunning { get; private set; }
        public bool IsUnscaled { get; private set; }
        public float TargetTime { get; private set; }
        public float CurrentTime { get; protected set; }

        public abstract bool IsFinished { get; }
        public abstract void Tick(float dt);

        public event Action<float> TimeChanged = delegate { };
        public event Action TimerStarted = delegate { };
        public event Action TimerStoped = delegate { };

        protected Timer(float targetTimeInSeconds, bool isUnscaled = false)
        {
            IsUnscaled = isUnscaled;
            TargetTime = targetTimeInSeconds;
        }

        public void Start()
        {
            CurrentTime = TargetTime;
            if (!IsRunning)
            {
                IsRunning = true;
                TimerManager.Instance.RegisterScaledTimer(this);
                TimerStarted.Invoke();
            }
        }

        public void Stop()
        {
            if (IsRunning)
            {
                IsRunning = false;
                TimerManager.Instance.DeregisterScaledTimer(this);
                TimerStoped.Invoke();
            }
        }

        public void Dispose()
        {
            TimerManager.Instance.DeregisterScaledTimer(this);
        }

        protected void Update() => TimeChanged.Invoke(CurrentTime);
    }
}
