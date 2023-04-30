using UnityEngine;

namespace Progfish.Boat {

    [CreateAssetMenu(menuName = "Boat/Boat Attack Data")]
    public class BoatAttackData : ScriptableObject {


        public BoatAttackDataInstance GetInstanceWithAdditionalParameters(
            ) {
            return new BoatAttackDataInstance() {

            };
        }
    }

    public struct BoatAttackDataInstance {
    }

}