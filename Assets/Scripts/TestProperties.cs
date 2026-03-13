using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace UniExtension
{
    public class TestProperties : MonoBehaviour
    {
        [SerializeField] private IntRange _myRange;
        [SerializeField] private Vector2 _vec;

        private List<int> _elements = new List<int>() { 10, 20, 30, 40, 50, 60 };
        private Timer timer;

        private void Awake()
        {
            Debug.Log(_elements.GetRandomElement());
            timer = new CountdownTimer(60f);
            timer.Start();
            timer.TimeChanged += Timer_TimeChanged;
        }

        private void OnDestroy()
        {
            timer.Stop();
        }

        private void Timer_TimeChanged(float obj)
        {
            Debug.Log(obj);
        }
    }
}
