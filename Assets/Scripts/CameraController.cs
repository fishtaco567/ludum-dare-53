using Unity.Mathematics;
using UnityEngine;

namespace Progfish.Boat {

    public class CameraController : MonoBehaviour {
    
        public GameObject target;
        public float3 offset;

        public float easing;

        private void LateUpdate() {
            float3 curPos = transform.position;
            float3 otherPos = target.transform.position;

            Follow(ref curPos, in otherPos, easing, in offset);

            transform.position = curPos;
        }

        private void Follow(ref float3 curPosition, in float3 otherPosition, float easing, in float3 offset) {
            curPosition = math.lerp(curPosition, otherPosition + offset, easing);
        }

    }

}