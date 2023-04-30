using UnityEngine;

namespace Progfish.Boat {

    [CreateAssetMenu(menuName = "Boat/Boat Movement Data")]
    public class BoatMovementData : ScriptableObject {
        public float turnSpeed;
        public float maxSpeed;
        public float acceleration;
        public float jumpTime;
        public float buoyancy;

        public float verticalDrag;
        public float planarDragInWater;
        public float planarDragInAir;

        public float maxSplashSize;
        public float maxSplashIntensity;
    
        public BoatMovementDataInstance GetInstanceWithAdditionalParameters(
            float handlingModifier = 1, float accelerationModifier = 1, float speedModifier = 1, float jumpModifier = 1,
            float buoyancyModifier = 1, float dragModifier = 1
            ) {
            return new BoatMovementDataInstance {
                turnSpeed = turnSpeed * handlingModifier,
                maxSpeed = maxSpeed * speedModifier,
                acceleration = acceleration * accelerationModifier,
                jumpSpeed = GetJumpSpeed(jumpTime * jumpModifier),
                buoyancy = buoyancy * buoyancyModifier,

                verticalDrag = verticalDrag * dragModifier,
                planarDragInWater = planarDragInWater * dragModifier,
                planarDragInAir = planarDragInAir * dragModifier,

                maxSplashSize = maxSplashSize * speedModifier,
                maxSplashIntensity = maxSplashIntensity * speedModifier,
            };
        }

        private float GetJumpSpeed(float time) {
            var gravity = GlobalSOProvider.Instance.consts.gravity;

            return 2 * gravity * time;
        }
    }

    public struct BoatMovementDataInstance {
        public float turnSpeed;
        public float maxSpeed;
        public float acceleration;
        public float jumpSpeed;
        public float buoyancy;

        public float verticalDrag;
        public float planarDragInWater;
        public float planarDragInAir;

        public float maxSplashSize;
        public float maxSplashIntensity;
    }

}