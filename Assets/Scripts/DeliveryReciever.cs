using UnityEngine;

namespace Progfish.Boat {

    public class DeliveryReciever : MonoBehaviour {

        public enum Mode {
            BlackHole,
            ToSource
        }

        public float timeToAccept;
        public Mode mode;
        
        public float currentTime;

        private Collider2D curOther;

        private DeliverySource thisSource;

        private void Start() {
            thisSource = GetComponent<DeliverySource>();
        }

        public void OnTriggerStay2D(Collider2D other) {
            if(thisSource != null && mode != Mode.BlackHole && !thisSource.CanAccept()) {
                return;
            }

            if(curOther == other) {
                var otherSource = other.GetComponent<DeliverySource>();
                if(otherSource == null) {
                    return;
                }

                if(!otherSource.CanTake()) {
                    return;
                }

                currentTime += Time.deltaTime;

                if(currentTime > timeToAccept && otherSource.TryTake(out var packet)) {
                    if(mode == Mode.ToSource) {
                        thisSource.Give(packet);
                    }
                }
                return;
            } else {
                currentTime = 0;
                curOther = other;
            }
        }

        public void OnTriggerExit2D(Collider2D other) {
            curOther = null;
            currentTime = 0;
        }

    }

}