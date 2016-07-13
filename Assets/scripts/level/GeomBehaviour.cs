using UnityEngine;
using System.Collections;
using level.data;
using level;

namespace level
{
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    public class GeomBehaviour : MonoBehaviour
    {

        public void Create(GeomData geomData, LevelBounds levelBounds)
        {
            MakeMesh(geomData, levelBounds);
            if (geomData.geomType != GeomType.Background)
                MakeCollider(geomData, levelBounds);
            SetMaterial(LoadMaterial(geomData.geomType.ToString()));
        }

        private Material LoadMaterial(string materialName)
        {
            return Resources.Load("materials/" + materialName, typeof(Material)) as Material;
        }

        private void MakeMesh(GeomData geomData, LevelBounds levelBounds)
        {
            MeshFilter mf = GetComponent<MeshFilter>();
            GeomMeshBuilder gmb = new GeomMeshBuilder(geomData, levelBounds);
            mf.mesh.vertices = gmb.Vertices();
            mf.mesh.triangles = gmb.Triangles();
            mf.mesh.RecalculateBounds();
            mf.mesh.RecalculateNormals();
        }

        private void SetMaterial(Material material)
        {
            MeshRenderer mr = GetComponent<MeshRenderer>();
            mr.material = material;
        }

        private void MakeCollider(GeomData geomData, LevelBounds levelBounds)
        {
            float edgeY = 0;
            if (geomData.geomPosition == GeomPosition.Top)
                edgeY = levelBounds.GeomTopEdge;
            else if (geomData.geomPosition == GeomPosition.Bottom)
                edgeY = levelBounds.GeomBottomEdge;



            gameObject.AddComponent<PolygonCollider2D>();
            PolygonCollider2D pg2d = gameObject.GetComponent<PolygonCollider2D>();

            Vector2 newFirst = new Vector2(geomData.points[0].x, edgeY);
            Vector2 newLast = new Vector2(geomData.points[geomData.points.Length - 1].x, edgeY);

            Vector2[] collisionPoints = new Vector2[geomData.points.Length + 2];
            collisionPoints[0] = newFirst;
            collisionPoints[collisionPoints.Length - 1] = newLast;
            System.Array.Copy(geomData.points, 0, collisionPoints, 1, geomData.points.Length);
            pg2d.points = collisionPoints;
        }
    }
}
