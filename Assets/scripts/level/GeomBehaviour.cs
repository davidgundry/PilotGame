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

        public void Create(GeomData geomData)
        {
            MakeMesh(geomData);
            SetMaterial(LoadMaterial(geomData.geomType.ToString()));
        }

        private Material LoadMaterial(string materialName)
        {
            return Resources.Load("materials/" + materialName, typeof(Material)) as Material;
        }

        private void MakeMesh(GeomData geomData)
        {
            MeshFilter mf = GetComponent<MeshFilter>();
            GeomMeshBuilder gmb = new GeomMeshBuilder(geomData);
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
    }
}
