using System.Collections.Generic;
using UnityEngine;

namespace UniExtension
{
    public class TestProperties : MonoBehaviour
    {
        [SerializeField] private IntRange _myRange;
        [SerializeField] private Vector2 _vec;

        private List<int> _elements = new List<int>() { 10, 20, 30, 40, 50, 60 };

        private void Awake()
        {
            Debug.Log(_elements.GetRandomElement());
        }
    }
}
