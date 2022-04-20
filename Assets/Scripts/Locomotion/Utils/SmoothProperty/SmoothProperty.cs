using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Locomotion.Utils.SmoothProperty
{
    public class SmoothProperty
    {
        private readonly Queue<Vector3> _lastValues;
        private readonly int _framesToCount;
        private readonly float[] _frameWeight;
    
        public Vector3 SmoothValue { get; private set; }

        public SmoothProperty(int framesToCount)
        {
            _framesToCount = framesToCount;
            _lastValues = new Queue<Vector3>();
            var frameWeightDistribution = new GaussianWeightDistribution(1.5f);
            _frameWeight = frameWeightDistribution.GetWeight(framesToCount);
        }

        public void AddFrameRecord(Vector3 currentValue)
        {
            _lastValues.Enqueue(currentValue);
            if (_lastValues.Count > _framesToCount)
                _lastValues.Dequeue();

            SmoothValue = Vector3.zero;
            var frameValues = _lastValues.Reverse().ToArray();
            for(int i = 0; i < frameValues.Length; i++)
            {
                SmoothValue += frameValues[i] * _frameWeight[i];
            }
        }
    }
}
