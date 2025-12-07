

using UnityEngine;

namespace Utilities
{
    public static class EasingFunctions
    {
        private const float _c1 = 1.70158f;
        
        private const float _c3 = _c1 + 1.0f;
        private const float _c4 = (2.0f * Mathf.PI) / 3.0f;
        
        // Taken from https://easings.net/#easeInBack 
        public static float EaseInBack(float t)
        {
            return _c3 * t * t * t - _c1 * t * t;
        }

        public static float EaseOutBack(float t)
        {
            return 1.0f + _c3 * Mathf.Pow(t - 1.0f, 3.0f) + _c1 * Mathf.Pow(t - 1.0f, 2.0f);
        }

        // Taken from https://easings.net/#easeOutQuart
        public static float EaseOutQuart(float t)
        {
            return 1.0f - UnityEngine.Mathf.Pow(1.0f - t, 4.0f);
        }

        // Taken from https://easings.net/#easeOutExpo
        public static float EaseOutExpo(float t)
        {
            if (t == 1.0f) return 1.0f;
            return 1.0f - Mathf.Pow(2.0f, -10.0f * t);
        }

        // Modification of the EaseOutExpo function
        public static float EaseOutFastExpo(float t)
        {
            return 1.0f - Mathf.Pow(10.0f, -10.0f * t);
        }

        // Taken from https://easings.net/#easeOutQuint
        public static float EaseOutQuint(float t)
        {
            return 1.0f - Mathf.Pow(1.0f - t, 5.0f);
        }
        
        public static float EaseOutDouble(float t)
        {
            return 1.0f - Mathf.Pow(1.0f - t, 10.0f);
        }

        public static float EaseInQuat(float t)
        {
            return t*t;
        }

        // Taken from https://easings.net/#easeOutElastic
        public static float EaseOutElastic(float t)
        {
            if (t == 0.0f) return 0.0f;
            if (t == 1.0f) return 1.0f;
            
            return Mathf.Pow(2.0f, -10.0f * t) * Mathf.Sin((t * 10.0f - 0.75f) * _c4) + 1.0f;
        }

        public static float NormalizeParabolaNotConvex(float t)
        {
            return -4.0f*t*t + 4.0f*t;
        }

        public static float EaseInBounce(float t)
        {
            return -4.0f * Mathf.Pow(t, 3.0f - Mathf.Pow(t, 4.0f)) + 5.0f*t*t;
        }

    }
}