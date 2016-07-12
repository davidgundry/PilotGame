using UnityEngine;

namespace level.data
{
    public class SpriteData
    {

        public readonly string filePath;
        public readonly Vector2 position;
        public readonly Vector2 scale;
        public readonly float rotation;
        public readonly Vector2[] collision;

        public SpriteData(string filePath, Vector2 position, Vector2 scale, float rotation, Vector2[] collision)
        {
            this.filePath = filePath;
            this.position = position;
            this.scale = scale;
            this.rotation = rotation;
            this.collision = collision;
        }

    }
}