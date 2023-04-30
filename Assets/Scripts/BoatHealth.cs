using UnityEngine;

namespace Progfish.Boat {

    public class BoatHealth : MonoBehaviour {

        private BoatHealthState state;
        private BoatHealthDataInstance data;

        private void Start() {
            data = GlobalSOProvider.Instance.baseBoatHealthData.GetInstanceWithAdditionalParameters(1, 1);
            ResetHealth();
        }

        private void Update() {
            DoHealthBehavior(ref state, in data);
        }

        private static void DoHealthBehavior(ref BoatHealthState state, in BoatHealthDataInstance data) {
            state.health += data.regenRate;

            if(state.health <= 0) {
                state.health = 0;
                state.isDead = true;
            }
        }

        public void Damage(float damage) {
            state.health -= damage;
        }

        public bool IsDead() => state.isDead;

        public void ResetHealth() {
            state = new BoatHealthState() {
                health = data.boatHealth,
                isDead = false,
            };
        }

    }

    public struct BoatHealthState {
        public float health;
        public bool isDead;
    }

}