using Unity.Mathematics;

namespace Progfish.Boat {

    public struct Input {
        public AttackInput attack;
        public MovementInput movement;
    }

    public struct AttackInput {
        public float2 axes;
        
        public bool shootPressed;
        public bool shootHeld;
        public float shootHeldTime;
        
        public bool targetPressed;
        public bool targetHeld;
        public float targetHeldTime;

        public float switchAxis;

        public float2? targetScreenPos;
    }

    public struct MovementInput {
        public float2 axes;
        public bool jumpButtonPressed;
        public bool jumpButtonHeld;
    }

}