using UnityEngine;

namespace Progfish.Boat {

    public class BoatHealthData : ScriptableObject {
    
        public float boatHealth;
        public float regenRate;

        public BoatHealthDataInstance GetInstanceWithAdditionalParameters(
            float healthModifier, float regenModifier) {
            return new BoatHealthDataInstance {
                boatHealth = boatHealth,
                regenRate = regenRate,
            };
        }

    }

    public struct BoatHealthDataInstance {
        public float boatHealth;
        public float regenRate;
    }

}