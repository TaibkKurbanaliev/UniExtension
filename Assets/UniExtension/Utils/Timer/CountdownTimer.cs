using UnityEngine;

namespace UniExtension
{
    public class CountdownTimer : Timer
    {
        public CountdownTimer(float targetTime, bool isUnscaled = false) : base(targetTime, isUnscaled)
        {
        }

        public override bool IsFinished => CurrentTime <= 0;

        public override void Tick(float dt)
        {
            if (IsRunning && CurrentTime > 0)
            {
                CurrentTime -= dt;
                Update();
            }
                
            if (IsRunning && CurrentTime <= 0)
            {
                Stop();
            }
        }
    }
}
