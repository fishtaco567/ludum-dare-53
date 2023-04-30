using UnityEngine;
using Rewired;
using Progfish.Boat.Graphics;

namespace Progfish.Boat {

    public class SingletonCreator : MonoBehaviour {

        public GlobalSOProvider soProviderPrefab;
        public PlayerInputManager inputManagerPrefab;
        public InputManager rewiredInputManager;
        public WaterSimulation waterSim;

        private void Awake() {
            TryInstantiate(soProviderPrefab);
            TryInstantiate(inputManagerPrefab);
            TryInstantiate(rewiredInputManager);
            TryInstantiate(waterSim);

            Destroy(this.gameObject);
        }

        private void TryInstantiate<T>(T prefab) where T : Component {
            if(FindObjectOfType<T>() == null) {
                Instantiate(prefab.gameObject);
            }
        }

    }

}