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

        public Vector3[] Vertices()
        {
            Vector3[] mesh = new Vector3[geomData.points.Length * 2];
            for (int i = 0; i < geomData.points.Length; i++)
            {
                mesh[i * 2] = new Vector3(geomData.points[i].x,geomData.points[i].y,zPosition);
                mesh[i * 2 + 1] = new Vector3(mesh[i * 2].x, edgeY,zPosition); 
            }

            return mesh;
        }

        public int[] Triangles()
        {
             bool odd = false;
             switch (geomData.geomPosition)
             {
                 case GeomPosition.Bottom:
                     odd = true;
                     break;
                 case GeomPosition.Top:
                     odd = false;
                     break;
             }

            int triangleCount = (geomData.points.Length - 1) * 2;
            int[] tris = new int[triangleCount*3];
           
            for (int i = 0; i < triangleCount; i++)
            {
                for (int j=0;j<3;j++)
                {
                    if (odd)
                        tris[i * 3 + j] = i + 2 - j;
                    else
                        tris[i * 3 + j] = i + j;
                }
                odd = !odd;
            }
            return tris;
        }

    }
}