using UnityEngine;
using System.Collections;
using level.data;

namespace level
{
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    public class FinishLineBehaviour : MonoBehaviour
    {

        public void Create(LevelData levelData, LevelBounds levelBounds)
        {
            MakeMesh(levelBounds);
            SetMaterial(LoadMaterial());
            transform.position = new Vector3(levelData.length, 0, 0);
        }

        private Material LoadMaterial()
        {
            return Resources.Load("materials/flagstring", typeof(Material)) as Material;
        }

        private void MakeMesh(LevelBounds levelBounds)
        {
            MeshFilter mf = GetComponent<MeshFilter>();
            mf.mesh.vertices = Vertices(levelBounds);
            mf.mesh.triangles = Triangles();
            mf.mesh.RecalculateBounds();
            mf.mesh.RecalculateNormals();
            mf.mesh.uv = UVs();
        }

        private Vector3[] Vertices(LevelBounds levelBounds)
        {
            return new Vector3[]
            {
                new Vector3(-0.2f,levelBounds.GeomTopEdge,0),
                new Vector3(0.2f,levelBounds.GeomTopEdge,0),
                new Vector3(0.2f,levelBounds.GeomBottomEdge,0),
                new Vector3(-0.2f,levelBounds.GeomBottomEdge,0)
            };
        }

        private int[] Triangles()
        {
            return new int[] {0,1,2,3,0,2};
        }

        private Vector2[] UVs()
        {
            return new Vector2[]
            {
                new Vector3(0,1),
                new Vector3(1,1),
                new Vector3(1,0),
                new Vector3(0,0)
            };
        }

        private void SetMaterial(Material material)
        {
            MeshRenderer mr = GetComponent<MeshRenderer>();
            mr.material = material;
        }

    }
}
