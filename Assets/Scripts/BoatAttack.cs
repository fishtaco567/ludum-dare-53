using Progfish.Boat.Graphics;
using Unity.Mathematics;
using UnityEngine;

namespace Progfish.Boat {
    public class BoatAttack : MonoBehaviour {

        private GameConsts gameConsts;
        private BoatAttackDataInstance data;
        private BoatAttackState state;
        WaterSimulation waterSim;

        private void Start() {
            gameConsts = GlobalSOProvider.Instance.consts;
            waterSim = WaterSimulation.Instance;

            data = GlobalSOProvider.Instance.baseBoatAttackData.GetInstanceWithAdditionalParameters();
        }

        private void Update() {
            var input = PlayerInputManager.Instance.GetPlayerInput(0);

            DoBoatAttack(ref state, in data, in input.attack);
        }

        private static void DoBoatAttack(ref BoatAttackState state, in BoatAttackDataInstance data, in AttackInput input) {
        
        }

        public void ResetAttackState() {
            state = new BoatAttackState() {
                 
            };
        }
    }

    public struct BoatAttackState {
        public static BoatAttackState Default() {
            return new BoatAttackState() {

            };
        }
    }
}
