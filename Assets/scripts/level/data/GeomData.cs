using UnityEngine;

namespace level.data
{
    public enum GeomType
    {
        Background,
        Hills,
        Mountains,
        Desert,
        Ocean,
        Cave
    }

    public enum GeomPosition
    {
        Top,
        Bottom
    }

    public class GeomData
    {

        public readonly GeomType geomType;
        public readonly GeomPosition geomPosition;
        public readonly Vector2[] points;

        public GeomData(GeomType geomType, GeomPosition geomPosition, Vector2[] points)
        {
            this.geomType = geomType;
            this.geomPosition = geomPosition;
            this.points = points;
        }

    }
}