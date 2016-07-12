using UnityEngine;

namespace level
{
    public class SpriteData
    {

        readonly string filePath;
        readonly Vector2 position;
        readonly Vector2 scale;
        readonly float rotation;
        readonly Vector2[] collision;

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