using UnityEngine;

namespace Progfish.Boat {

    public class DeliverySource : MonoBehaviour {
    
        private DeliveryPacket? currentPacket;
        public DeliveryPacket? CurrentPacket { get { return currentPacket; } }

        public SpriteRenderer spriteRenderer;

        public bool allowOverwrite;

        public bool initDebug;
        public Sprite debugSprite;

        private void Awake() => UpdateGraphics();

        private void Start() {
            if(initDebug) {
                currentPacket = new DeliveryPacket() {
                    token = "Mine",
                    sprite = debugSprite,
                };
                UpdateGraphics();
            }
        }

        public bool CanTake() {
            return currentPacket.HasValue;
        }

        public bool TryTake(out DeliveryPacket packet) {
            if(currentPacket.HasValue) {
                packet = currentPacket.Value;
                currentPacket = null;
                UpdateGraphics();
                return true;
            }

            packet = default;
            return false;
        }

        public void Give(DeliveryPacket packet) {
            currentPacket = packet;

            UpdateGraphics();
        }

        public bool CanAccept() {
            return !currentPacket.HasValue || allowOverwrite;
        }

        private void UpdateGraphics() {
            if(currentPacket.HasValue) {
                spriteRenderer.sprite = currentPacket.Value.sprite;
            } else {
                spriteRenderer.sprite = null;
            }
        }

    }

}