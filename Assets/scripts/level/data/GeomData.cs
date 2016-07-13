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
        public readonly string name;
        public readonly int pivotStartPoints;
        public readonly int pivotEndPoints;
        public int totalPivotPoints { get { return pivotStartPoints + pivotEndPoints; } }

        public GeomData(string name, GeomType geomType, GeomPosition geomPosition, Vector2[] points, int pivotStartPoints, int pivotEndPoints)
        {
            this.name = name;
            this.geomType = geomType;
            this.geomPosition = geomPosition;
            this.points = points;
            this.pivotStartPoints = pivotStartPoints;
            this.pivotEndPoints = pivotEndPoints;
        }

    }
}