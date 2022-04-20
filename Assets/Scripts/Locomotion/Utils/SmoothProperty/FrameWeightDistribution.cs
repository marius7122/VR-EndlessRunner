using System;
using System.Linq;
using UnityEngine;

namespace Locomotion.Utils.SmoothProperty
{
    public interface IFrameWeightDistribution
    {
        float[] GetWeight(int frames);
    }
    
    
    public class GaussianWeightDistribution : IFrameWeightDistribution
    {
        private readonly float _sigma;
        
        public GaussianWeightDistribution(float sigma)
        {
            _sigma = sigma;
        }

        public float[] GetWeight(int frames)
        {
            var weights = new float[frames];
            for (int i = 0; i < frames; i++)
            {
                weights[i] = GaussianFormula(i, _sigma);
            }
            NormalizeArray(weights);
            return weights;
        }

        private static void NormalizeArray(float[] arr)
        {
            var sum = arr.Sum();
            for (int i = 0; i < arr.Length; i++)
                arr[i] /= sum;
        }
        
        private static float GaussianFormula(float dist, float sigma)
        {
            var topPart = Mathf.Pow((float)Math.E, -(dist * dist) / (2 * sigma * sigma));
            var bottomPart = Mathf.Sqrt(2 * Mathf.PI * sigma * sigma + 1);
            return topPart / bottomPart;
        }
    }
}