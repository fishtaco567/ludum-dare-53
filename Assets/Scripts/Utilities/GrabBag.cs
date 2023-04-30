using System.Collections.Generic;

namespace Progfish.Utils {
    public class GrabBag<T> {
        public List<T> things;
        public List<float> chances;

        private float sum;

        public GrabBag() {
            things = new List<T>();
            chances = new List<float>();
            sum = 0;
        }

        public void AddThing(T thing, float chance) {
            things.Add(thing);
            chances.Add(chance);
            sum += chance;
        }

        public void Reset() {
            things.Clear();
            chances.Clear();
            sum = 0;
        }

        public T Grab(SRandom rand) {
            var x = rand.RandomFloatInRange(0, sum);

            int chosen = 0;
            for(int i = 0; i < things.Count; i++) {
                chosen = i;
                x -= chances[i];
                if(x <= 0) {
                    break;
                }
            }

            T thing = things[chosen];
            sum -= chances[chosen];
            things.RemoveAt(chosen);
            chances.RemoveAt(chosen);
            return thing;
        }

    }
}
