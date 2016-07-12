using level.data;
using UnityEngine;

namespace level
{
    public class GeomMeshBuilder
    {
        GeomData geomData;
        float edgeY;
        const float geomBottomEdge = -10;
        const float geomTopEdge = 100;

        public GeomMeshBuilder(GeomData geomData)
        {
            this.geomData = geomData;
            SetEdge();
        }

        private void SetEdge()
        {
            switch (geomData.geomPosition)
            {
                case GeomPosition.Bottom:
                    edgeY = geomBottomEdge;
                    break;
                case GeomPosition.Top:
                    edgeY = geomTopEdge;
                    break;
            }
        }

        public Vector2[] Mesh()
        {
            Vector2[] mesh = new Vector2[geomData.points.Length * 2];
            for (int i = 0; i < geomData.points.Length; i++)
            {
                mesh[i * 2] = geomData.points[i];
                mesh[i * 2 + 1] = new Vector2(mesh[i * 2].x, edgeY); 
            }

            return mesh;
        }

        public int[] Tris()
        {
            int[] tris = new int[geomData.points.Length];
            for (int i = 0; i < geomData.points.Length; i++)
            {
                for (int j=0;j<3;j++)
                {
                    tris[i * 3 + j] = i+j;
                }
            }
            return tris;
        }

    }
}