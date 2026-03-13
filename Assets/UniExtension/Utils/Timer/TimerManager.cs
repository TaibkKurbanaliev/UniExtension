using System.Collections.Generic;
using UnityEngine;

namespace UniExtension
{
    public class TimerManager : MonoBehaviour
    {
        private readonly List<Timer> _scaledTimers = new();

        public static TimerManager Instance;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void Initialize()
        {
            if (Instance != null) return;

            GameObject obj = new GameObject("TimerManager");
            obj.AddComponent<TimerManager>();
            DontDestroyOnLoad(obj);
        }

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void Update()
        {
            UpdateTimers();
        }

        public void RegisterScaledTimer(Timer timer) => _scaledTimers.Add(timer);
        public void DeregisterScaledTimer(Timer timer) => _scaledTimers.Remove(timer);

        public void UpdateTimers()
        {
            foreach (var timer in new List<Timer>(_scaledTimers))
                timer.Tick(timer.IsUnscaled ? Time.unscaledDeltaTime : Time.deltaTime);
        }
    }
}
