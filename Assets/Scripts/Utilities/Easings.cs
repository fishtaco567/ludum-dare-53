using UnityEngine;

namespace Progfish.Utils {
    public static class Easings {

        public static float EaseInOutSine(float x) {
            return -(Mathf.Cos(Mathf.PI * x) - 1) / 2;
        }

        public static float EaseInSine(float x) {
            return 1 - Mathf.Cos((Mathf.PI * x) / 2);
        }

        public static float EaseOutSine(float x) {
            return Mathf.Sin((Mathf.PI * x) / 2);
        }

        public static float EaseInQuad(float x) {
            return x * x;
        }

        public static float EaseOutQuad(float x) {
            return 1 - (1 - x) * (1 - x);
        }

        public static float EaseInOutQuad(float x) {
            return x < 0.5 ? 2 * x * x : 1 - ((2 * x - 2) * (2 * x - 2)) / 2;
        }

        public static float Identity(float x) {
            return x;
        }
        public static float EaseOutBack(float x) {
            float c1 = 1.70158f;
            float c3 = c1 + 1;

            return 1 + c3 * Mathf.Pow(x - 1, 3) + c1 * Mathf.Pow(x - 1, 2);
        }
    }

}