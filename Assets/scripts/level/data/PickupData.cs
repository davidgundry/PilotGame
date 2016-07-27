using UnityEngine;
using System.Collections;

public enum PickupType
{
    Fuel,
    Repair,
    Speed,
    Coin
}

namespace level.data
{

    public class PickupData
    {
        public readonly string name;
        public readonly PickupType pickupType;
        public readonly Vector2 position;
        public readonly float rotation;

        public PickupData(string name, PickupType pickupType, Vector2 position, float rotation)
        {
            this.name = name;
            this.pickupType = pickupType;
            this.position = position;
            this.rotation = rotation;
        }

    }
}