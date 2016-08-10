using UnityEngine;
using System.Collections;
using level.data;

namespace level.behaviours
{
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    public class CloudLineBehaviour : MonoBehaviour
    {

        float zPosition;

        public void Create(LevelData levelData, float zPosition)
        {
            this.zPosition = zPosition;

            float inset = 0;
            float cloudLineWidth = 30;
            float overlap = 4;
            while (inset < levelData.length+150)
            {
                //MakeMesh(cloudLineWidth);
                GameObject c = new GameObject();
                c.name = "CloudLineComponent";
                
                c.AddComponent<SpriteRenderer>();
                SpriteRenderer spriteRenderer = c.GetComponent<SpriteRenderer>();
                spriteRenderer.sprite = Resources.Load<Sprite>("sprites/cloudline");
                //SetMaterial(LoadMaterial());
                c.transform.position = new Vector3(inset, levelData.height+3, 0);
                c.transform.localScale = new Vector3(4, 4, 4);
                inset += cloudLineWidth-overlap;
                c.transform.parent = this.transform;
            }

            
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
