using level.data;
using UnityEngine;

namespace level
{
    public class GeomMeshBuilder
    {
        GeomData geomData;
        float edgeY;
        const float backgroundZPosition = 10;
        const float defaultZPosition = 0;

        private float zPosition = 0;

        public GeomMeshBuilder(GeomData geomData, LevelBounds levelBounds)
        {
            this.geomData = geomData;
            SetEdge(levelBounds);
            SetZ();
        }

        private void SetEdge(LevelBounds levelBounds)
        {
            switch (geomData.geomPosition)  
            {
                case GeomPosition.Bottom:
                    edgeY = levelBounds.GeomBottomEdge;
                    break;
                case GeomPosition.Top:
                    edgeY = levelBounds.GeomTopEdge;
                    break;
            }
        }

        private void SetZ()
        {
            if (geomData.geomType == GeomType.Background)
                zPosition = backgroundZPosition;
            else
                zPosition = defaultZPosition;
        }

        public int VerticesCount()
        {
            return (geomData.points.Length * 2) - (geomData.totalPivotPoints);
        }

        public Vector3[] Vertices()
        {
            Vector3[] mesh = new Vector3[VerticesCount()];

            for (int i = 0; i < geomData.pivotStartPoints; i++)
            {
                mesh[i] = new Vector3(geomData.points[i].x, geomData.points[i].y, zPosition);
            }

            for (int i = 0; i < geomData.points.Length - (geomData.totalPivotPoints); i++)
            {
                mesh[(i * 2) + geomData.pivotStartPoints] = new Vector3(geomData.points[i + geomData.pivotStartPoints].x, edgeY, zPosition);
                mesh[(i * 2) + 1 + geomData.pivotStartPoints] = new Vector3(geomData.points[i + geomData.pivotStartPoints].x, geomData.points[i + geomData.pivotStartPoints].y, zPosition);

            }

            int meshIndex = 1;
            for (int i = geomData.points.Length - 1; i > geomData.points.Length - 1 - geomData.pivotEndPoints; i--)
            {
                mesh[mesh.Length-meshIndex] = new Vector3(geomData.points[i].x, geomData.points[i].y, zPosition);
                meshIndex++;
            }


            return mesh;
        }

        public int[] Triangles()
        {
            bool odd = false;
            switch (geomData.geomPosition)
            {
                case GeomPosition.Bottom:
                    odd = false;
                    break;
                case GeomPosition.Top:
                    odd = true;
                    break;
            }

            if (geomData.pivotStartPoints == 1)
                odd = !odd;

            int pivotTriangleCount = geomData.totalPivotPoints;
            int mainTriangleCount = (geomData.points.Length - geomData.totalPivotPoints - 1) * 2;
            int totalTriangleCount = pivotTriangleCount + mainTriangleCount;

            int[] tris = new int[totalTriangleCount * 3];


            for (int i = 0; i < totalTriangleCount; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (odd)
                        tris[i * 3 + j] = i + 2 - j;
                    else
                        tris[i * 3 + j] = i + j;
                }
                odd = !odd;
            }


            if (geomData.pivotEndPoints == 2)
            {
                int i = totalTriangleCount - 1;
                if (odd)
                {
                    tris[i * 3 + 0] = i + 2;
                    tris[i * 3 + 1] = i + 1;
                    tris[i * 3 + 2] = i - 1;
                }
                else
                {
                    tris[i * 3 + 0] = i - 1;
                    tris[i * 3 + 1] = i + 1;
                    tris[i * 3 + 2] = i + 2;
                }
            }
            return tris;
        }

        public Vector2[] UVs(TextureType textureType)
        {
            float xOffset = 0;
            switch (textureType)
            {
                case TextureType.OuterLine:
                    xOffset = 0.25f;
                    break;
                case TextureType.InnerLine:
                    xOffset = 0.5f;
                    break;
                case TextureType.InnerFill:
                    xOffset = 0.75f;
                    break;
            }
            float yOffset = 0;
            switch (geomData.geomType)
            {
                case GeomType.Hills:
                    yOffset = 0.95f;
                    break;
                case GeomType.Mountains:
                    yOffset = 0.85f;
                    break;
                case GeomType.Desert:
                    yOffset = 0.75f;
                    break;
                case GeomType.Cave:
                    yOffset = 0.65f;
                    break;
                case GeomType.Background:
                    yOffset = 0.55f;
                    break;
                case GeomType.Ocean:
                    yOffset = 0.45f;
                    break;
            }

            Vector2[] uvs = new Vector2[VerticesCount()];
            for (int i = 0; i < VerticesCount(); i++)
            {
                uvs[i] = new Vector2(xOffset, yOffset);
            }
            return uvs;
            
        }

    }


    public enum TextureType
    {
        OuterLine,
        InnerLine,
        InnerFill
    }

}