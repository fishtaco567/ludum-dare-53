using Progfish.Boat.Graphics;
using Unity.Mathematics;
using UnityEngine;

namespace Progfish.Boat {
    public class BoatMovement : MonoBehaviour {

        private GameConsts gameConsts;

        BoatMovementDataInstance data;
        BoatMovementState state;
        WaterSimulation waterSim;

        private void Start() {
            gameConsts = GlobalSOProvider.Instance.consts;
            data = GlobalSOProvider.Instance.baseBoatMovementData.GetInstanceWithAdditionalParameters();
            waterSim = WaterSimulation.Instance;

            ResetBoatMovement();
        }

        private void Update() {
            var input = PlayerInputManager.Instance.GetPlayerInput(0);

            DoBoatMovement(ref state, in data, in input.movement, gameConsts);

            CommitToGameObject(in state, this.gameObject);
            CommitToWaterSim(in state, in data, waterSim);
        }

        public void ResetBoatMovement() {
            state = BoatMovementState.WithPositionAndAngle(new float2(transform.position.x, transform.position.y), 0);
        }

        private static void CommitToGameObject(in BoatMovementState state, GameObject gameObject) {
            gameObject.transform.position = new float3(state.position.x, state.position.y, state.height);
            gameObject.transform.eulerAngles = new float3(0, 0, -state.angle);
        }

        private static void CommitToWaterSim(in BoatMovementState state, in BoatMovementDataInstance data, WaterSimulation waterSim) {
            var movementAngle = math.atan(state.velocity.y / state.velocity.x);
            var portion = math.lengthsq(state.velocity) / (data.maxSpeed * data.maxSpeed);
            if(portion > 0.00001f) {
                waterSim.StartSplash(SplashKind.BoatSplash, state.position, data.maxSplashSize, movementAngle, data.maxSplashIntensity * portion);

                if(math.length(state.velocity) > 0.05f) {
                    var angleRad = math.radians(state.angle);
                    var pointing = new float2(math.cos(angleRad), -math.sin(angleRad));
                    var angleDiff = math.abs(90 - Vector2.Angle(math.normalize(state.velocity), pointing)) / 90;
                    float slowdownExtraFactor = (1 - (angleDiff * angleDiff)) * 1f;
                    waterSim.StartSplash(SplashKind.BoatSplash, state.position + math.normalize(state.velocity) * 0.3f * slowdownExtraFactor,
                        data.maxSplashSize * slowdownExtraFactor, movementAngle, data.maxSplashIntensity * portion * slowdownExtraFactor);
                }
            }
        }

        private void DoBoatMovement(ref BoatMovementState state, in BoatMovementDataInstance data, in MovementInput input, GameConsts consts) {
            state.angle += data.turnSpeed * input.axes.x * Time.deltaTime;

            var angleRad = math.radians(state.angle);
            var pointing = new float2(math.cos(angleRad), -math.sin(angleRad));

            if(state.inWater) {
                state.velocity += data.acceleration * input.axes.y * pointing * Time.deltaTime;
            }

            if(input.jumpButtonPressed && state.inWater) {
                state.verticalVelocity = data.jumpSpeed;
                state.inWater = false;
            }

            state.verticalVelocity -= consts.gravity * Time.deltaTime;

            if(state.height < 0) {
                state.inWater = true;
                state.verticalVelocity += state.height * data.buoyancy;
            }

            state.verticalVelocity *= (1 - data.verticalDrag);

            if(state.inWater) {
                if(math.length(state.velocity) > 0.05f) {
                    var angleDiff = math.abs(90 - Vector2.Angle(math.normalize(state.velocity), pointing)) / 90;
                    float slowdownExtraFactor = (1 - (angleDiff * angleDiff)) * 3;
                    var sideSlowdown = state.velocity * data.planarDragInWater * slowdownExtraFactor;
                    state.velocity -= sideSlowdown;
                    state.velocity += pointing * math.length(sideSlowdown) * 0.8f;
                }

                state.velocity *= 1 - data.planarDragInWater;
            } else {
                state.velocity *= 1 - data.planarDragInAir;
            }

            var velocityMagnitude = math.length(state.velocity);

            if(velocityMagnitude > data.maxSpeed) {
                state.velocity = (state.velocity / velocityMagnitude) * data.maxSpeed;
            }

            state.position += state.velocity * Time.deltaTime;
        }
    }

    public struct BoatMovementState {
        public float2 position;
        public float2 velocity;
        public float angle;

        // Separable, doesn't interact with other axes
        public float height;
        public float verticalVelocity;

        public bool inWater;

        public static BoatMovementState WithPositionAndAngle(float2 position, float angle) {
            return new BoatMovementState {
                position = position,
                velocity = float2.zero,
                angle = angle,
                height = 0,
                verticalVelocity = 0,
                inWater = true,
            };
        }
    }
}
