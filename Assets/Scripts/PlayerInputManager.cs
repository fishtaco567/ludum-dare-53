using Progfish.Utils;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Assertions;

namespace Progfish.Boat {
    
    public class PlayerInputManager : Singleton<PlayerInputManager> {
       
        const int NUM_PLAYERS = 1;

        public Rewired.Player[] players;

        private void Awake() {
            DontDestroyOnLoad(this.gameObject);
        }

        private void Start() {
            players = new Rewired.Player[NUM_PLAYERS];

            for(int i = 0; i < NUM_PLAYERS; i++) {
                players[i] = Rewired.ReInput.players.GetPlayer(i);
            }
        }

        public Input GetPlayerInput(int i) {
            Assert.IsTrue(i >= 0 && i < NUM_PLAYERS);
        
            var player = players[i];

            var movement = new MovementInput { 
                axes = new float2(player.GetAxis(RewiredConsts.Action.PHorizontal),
                player.GetAxis(RewiredConsts.Action.PVertical)),
                jumpButtonPressed = player.GetButtonDown(RewiredConsts.Action.Jump),
                jumpButtonHeld = player.GetButton(RewiredConsts.Action.Jump),
            };

            var attack = new AttackInput {
                axes = new float2(player.GetAxis(RewiredConsts.Action.SHorizontal),
                player.GetAxis(RewiredConsts.Action.SVertical)),

                shootPressed = player.GetButtonDown(RewiredConsts.Action.Shoot),
                shootHeld = player.GetButton(RewiredConsts.Action.Shoot),
                shootHeldTime = (float) player.GetButtonTimePressed(RewiredConsts.Action.Shoot),

                targetPressed = player.GetButtonDown(RewiredConsts.Action.Shoot),
                targetHeld = player.GetButton(RewiredConsts.Action.Shoot),
                targetHeldTime = (float)player.GetButtonTimePressed(RewiredConsts.Action.Shoot),

                switchAxis = player.GetAxis(RewiredConsts.Action.Switch),

                targetScreenPos = Rewired.ReInput.controllers.Mouse.screenPosition,
            };

            var input = new Input {
                attack = attack,
                movement = movement,
            };

            return input;
        }

    }

}