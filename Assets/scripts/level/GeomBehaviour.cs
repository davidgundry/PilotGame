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
            {
                MakeCollider(geomData, levelBounds);
                SetPhysicsMaterial(LoadPhysicsMaterial("GroundPhysics"));
            }

            SetMaterial(LoadMaterial(geomData.geomType.ToString()));


            if (geomData.geomType != GeomType.Background)
                DuplicateMeshForOutline(geomData);
        }

        private Material LoadMaterial(string materialName)
        {
            return Resources.Load("materials/" + materialName, typeof(Material)) as Material;
        }

        private PhysicsMaterial2D LoadPhysicsMaterial(string materialName)
        {
            return Resources.Load("materials/" + materialName, typeof(PhysicsMaterial2D)) as PhysicsMaterial2D;
        }

        private void SetPhysicsMaterial(PhysicsMaterial2D material)
        {
            GetComponent<PolygonCollider2D>().sharedMaterial = material;
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

        private void DuplicateMeshForOutline(GeomData geomData)
        {
            MeshFilter mf = GetComponent<MeshFilter>();
            float lineThickness = 0.5f;

            GameObject outline = new GameObject();
            outline.name = "Line";
            outline.transform.SetParent(this.transform);
            outline.AddComponent<MeshFilter>();
            outline.AddComponent<MeshRenderer>();
            MeshFilter outlineChildMF = outline.GetComponent<MeshFilter>();
            outlineChildMF.mesh = mf.mesh;

            if (geomData.geomPosition == GeomPosition.Bottom)
                outline.transform.localPosition = new Vector3(0, -lineThickness, -0.1f);
            else if (geomData.geomPosition == GeomPosition.Top)
                outline.transform.localPosition = new Vector3(0, lineThickness, -0.1f);

            MeshRenderer outlineMR = outline.GetComponent<MeshRenderer>();
            outlineMR.material = LoadMaterial(geomData.geomType.ToString() + "Line");


            GameObject inner = new GameObject();
            inner.name = "Inner";
            inner.transform.SetParent(this.transform);
            inner.AddComponent<MeshFilter>();
            inner.AddComponent<MeshRenderer>();
            MeshFilter innerChildMF = inner.GetComponent<MeshFilter>();
            innerChildMF.mesh = mf.mesh;

            if (geomData.geomPosition == GeomPosition.Bottom)
                inner.transform.localPosition = new Vector3(0, -2 * lineThickness, -0.2f);
            else if (geomData.geomPosition == GeomPosition.Top)
                inner.transform.localPosition = new Vector3(0, 2 * lineThickness, -0.2f);

            MeshRenderer innerMR = inner.GetComponent<MeshRenderer>();
            if (geomData.geomType != GeomType.Desert)
                innerMR.material = LoadMaterial("Inner");
            else
                innerMR.material = LoadMaterial("DesertInner");
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
