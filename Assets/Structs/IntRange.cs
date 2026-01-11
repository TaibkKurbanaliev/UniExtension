using System;
using UnityEngine;

namespace UniExtension
{
    [Serializable] 
    public struct IntRange
    {
        [SerializeField] private int _min;
        [SerializeField] private int _max;

        public int max
        {
            get => _max;
            set
            {
                if (value <= _min)
                    _max = _min;

                _max = value;
            }
        }

        public int min
        {
            get => _min;
            set
            {
                if (value >= _max)
                    _min = _max;

                _min = value;
            }
        }
    }
}
