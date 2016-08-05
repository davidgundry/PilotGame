using UnityEngine;
using System.Collections;

namespace level.data
{

    public class HoopData
    {
        public readonly string name;
        public readonly Vector2 position;
        public readonly float rotation;
        public readonly Vector2 scale;

        public HoopData(string name, Vector2 position, float rotation, Vector2 scale)
        {
            this.name = name;
            this.position = position;
            this.rotation = rotation;
            this.scale = scale;
        }

    }
}