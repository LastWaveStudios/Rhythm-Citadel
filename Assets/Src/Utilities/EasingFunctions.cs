

namespace Utilities
{
    public static class EasingFunctions
    {
        // Taken from https://easings.net/#easeInBack 
        public static float EaseInBack(float t)
        {
            const float c1 = 1.70158f;
            const float c3 = c1 + 1.0f;

            return c3 * t * t * t - c1 * t * t;
        }

        // Taken from https://easings.net/#easeOutQuart
        public static float EaseOutQuart(float t)
        {
            return 1.0f - UnityEngine.Mathf.Pow(1.0f - t, 4.0f);
        }

    }
}