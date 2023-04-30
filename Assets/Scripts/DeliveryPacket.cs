using System;
using UnityEngine;

namespace Progfish.Boat {

    public struct DeliveryPacket : IEquatable<DeliveryPacket> {
        public string token;
        public Sprite sprite;

        public bool Equals(DeliveryPacket other) {
            return token == other.token;
        }
    }

}