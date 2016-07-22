using UnityEngine;
using System.Collections;
using level.data;

namespace level
{
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    public class CloudLineBehaviour : MonoBehaviour
    {

        float zPosition;

        public void Create(LevelData levelData, float zPosition)
        {
            this.zPosition = zPosition;
            MakeMesh(levelData.length);
            SetMaterial(LoadMaterial());
            transform.position = new Vector3(0, levelData.height, 0);
        }

        private Material LoadMaterial()
        {
            return Resources.Load("materials/cloudline", typeof(Material)) as Material;
        }

        private void MakeMesh(float levelWidth)
        {
            MeshFilter mf = GetComponent<MeshFilter>();
            mf.mesh.vertices = Vertices(levelWidth);
            mf.mesh.triangles = Triangles();
            mf.mesh.RecalculateBounds();
            mf.mesh.RecalculateNormals();
            mf.mesh.uv = UVs();
        }

        private Vector3[] Vertices(float levelWidth)
        {
            return new Vector3[]
            {
                new Vector3(0,4f,zPosition),
                new Vector3(levelWidth,4f,zPosition),
                new Vector3(levelWidth,-4f,zPosition),
                new Vector3(0,-4f,zPosition)
            };
        }

        private int[] Triangles()
        {
            return new int[] { 0, 1, 2, 3, 0, 2 };
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
