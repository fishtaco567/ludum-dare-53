using Progfish.Boat.Graphics;
using Progfish.Utils;

namespace Progfish.Boat {

    public class GlobalSOProvider : Singleton<GlobalSOProvider> {
    
        public GameConsts consts;
        public BoatMovementData baseBoatMovementData;
        public BoatHealthData baseBoatHealthData;
        public BoatAttackData baseBoatAttackData;

        private void Awake() {
            DontDestroyOnLoad(this.gameObject);
        }

    }

}