using UnityEngine;
using System.Collections.Generic;
using level.data;
using player.behaviour;

namespace level.behaviours
{
    [RequireComponent(typeof(MeshFilter))]
    public class WaterBehaviour : GeomBehaviour
    {

        float[] xpositions;
        float[] ypositions;
        float[] velocities;
        float[] accelerations;
        LineRenderer Body;

        Mesh mesh;
        int quads;
        GameObject[] colliders;
        

        const float springconstant = 0.02f;
        const float damping = 0.04f;
        const float spread = 0.07f;
        const float z = 0f;

        float baseheight;
        float left;
        float bottom;

        private ParticleSystem particleSystem;
        private Material lineMaterial;

        public void SpawnWater(float Left, float Width, float Top, float Bottom)
        {
            int edgecount = Mathf.RoundToInt(Width)*5;
            int nodecount = edgecount + 1;

            Body = gameObject.AddComponent<LineRenderer>();
            Body.material = lineMaterial;
            Body.material.renderQueue = 1000;
            Body.SetVertexCount(nodecount);
            Body.SetWidth(0.7f, 0.7f);

            xpositions = new float[nodecount];
            ypositions = new float[nodecount];
            velocities = new float[nodecount];
            accelerations = new float[nodecount];

            colliders = new GameObject[edgecount];

            baseheight = Top;
            bottom = Bottom;
            left = Left;

            for (int i = 0; i < nodecount; i++)
            {
                ypositions[i] = Top + Random.value;
                xpositions[i] = Left + Width * i / edgecount;
                accelerations[i] = 0;
                velocities[i] = 0;
                Body.SetPosition(i, new Vector3(xpositions[i], ypositions[i], z));
            }


            List<Vector3> vertices = new List<Vector3>();
            List<Vector2> uvs = new List<Vector2>();
            List<int> tris = new List<int>();
            mesh = new Mesh();
            for (int i = 0; i < edgecount; i++)
            {
                vertices.Add(new Vector3(xpositions[i], ypositions[i], z));
                vertices.Add(new Vector3(xpositions[i + 1], ypositions[i + 1], z));
                vertices.Add(new Vector3(xpositions[i], bottom, z));
                vertices.Add(new Vector3(xpositions[i + 1], bottom, z));

                uvs.Add(new Vector2(0.25f, 0.45f));
                uvs.Add(new Vector2(0.25f, 0.45f));
                uvs.Add(new Vector2(0.25f, 0.45f));
                uvs.Add(new Vector2(0.25f, 0.45f));

                tris.AddRange(new int[6] { quads * 4 + 0, quads * 4 + 1, quads * 4 + 3, quads * 4 + 3, quads * 4 + 2, quads * 4 + 0 });
                quads++;
            }
            mesh.SetVertices(vertices);
            mesh.SetUVs(0,uvs);
            mesh.SetTriangles(tris,0);
            GetComponent<MeshFilter>().mesh = mesh;
        }

        void UpdateMeshes()
        {
            List<Vector3> vertices = new List<Vector3>();
            for (int i = 0; i < quads; i++)
            {
                vertices.Add(new Vector3(xpositions[i], ypositions[i], z));
                vertices.Add(new Vector3(xpositions[i + 1], ypositions[i + 1], z));
                vertices.Add(new Vector3(xpositions[i], bottom, z));
                vertices.Add(new Vector3(xpositions[i + 1], bottom, z));
            }
            mesh.SetVertices(vertices);
        }

        void FixedUpdate()
        {
            int mass = 1;
            for (int i = 0; i < xpositions.Length; i++)
            {
                float force = springconstant * (ypositions[i] - baseheight) + velocities[i] * damping;
                accelerations[i] = -force / mass;
                ypositions[i] += velocities[i];
                velocities[i] += accelerations[i];
                Body.SetPosition(i, new Vector3(xpositions[i], ypositions[i], z));
            }

            float[] leftDeltas = new float[xpositions.Length];
            float[] rightDeltas = new float[xpositions.Length];

            for (int j = 0; j < 8; j++)
            {
                for (int i = 0; i < xpositions.Length; i++)
                {
                    if (i > 0)
                    {
                        leftDeltas[i] = spread * (ypositions[i] - ypositions[i - 1]);
                        velocities[i - 1] += leftDeltas[i];
                    }
                    if (i < xpositions.Length - 1)
                    {
                        rightDeltas[i] = spread * (ypositions[i] - ypositions[i + 1]);
                        velocities[i + 1] += rightDeltas[i];
                    }
                }
            }

            for (int i = 0; i < xpositions.Length; i++)
            {
                if (i > 0)
                {
                    ypositions[i - 1] += leftDeltas[i];
                }
                if (i < xpositions.Length - 1)
                {
                    ypositions[i + 1] += rightDeltas[i];
                }
            }

            UpdateMeshes();
        }


        public void Splash(float xpos, float velocity)
        {
            if ((xpos >= xpositions[0]) && (xpos <= xpositions[xpositions.Length - 1]))
            {
                xpos -= xpositions[0];
                int index = Mathf.RoundToInt((xpositions.Length - 1) * (xpos / (xpositions[xpositions.Length - 1] - xpositions[0])));
                velocities[index] = velocity;

                //float lifetime =  0.93f + Mathf.Abs(velocity) * 0.07f;
                particleSystem.startSpeed = 8 + 4 * Mathf.Pow(Mathf.Abs(velocity), 0.5f);
                //particleSystem.startSpeed = 9 + 2 * Mathf.Pow(Mathf.Abs(velocity), 0.5f);
                //particleSystem.startLifetime = lifetime;
                Vector3 position = new Vector3(xpositions[index], ypositions[index] - 0.35f, 8);
                Quaternion rotation = Quaternion.LookRotation(new Vector3(xpositions[Mathf.FloorToInt(xpositions.Length / 2)], baseheight + 8, 5) - position);
                GameObject splish = Instantiate(particleSystem, position, rotation) as GameObject;
                Destroy(splish, 0.5f);
            }
            
        }

        public override void Create(GeomData geomData, LevelBounds levelBounds)
        {
            LevelSession levelSession = GameObject.FindObjectOfType<LevelSession>();
            particleSystem = levelSession.splashParticleSystem.GetComponent<ParticleSystem>();
            lineMaterial = levelSession.waterLineMaterial;

            float edge = levelBounds.GeomBottomEdge;
            if (geomData.geomPosition == GeomPosition.Top)
                edge = levelBounds.GeomTopEdge;

            SpawnWater(geomData.points[0].x, geomData.points[1].x - geomData.points[0].x, geomData.points[0].y, edge);

            //MakeMesh(geomData, levelBounds);

            MakeCollider(geomData, levelBounds);
            SetPhysicsMaterial(LoadPhysicsMaterial("GroundPhysics"));

            SetMaterial(LoadMaterial("Ground"));
        }

        private void MakeMesh(GeomData geomData, LevelBounds levelBounds)
        {
            MeshFilter mf = GetComponent<MeshFilter>();
            WaterMeshBuilder wmb = new WaterMeshBuilder(geomData, levelBounds);
            mf.mesh.vertices = wmb.Vertices();
            mf.mesh.triangles = wmb.Triangles();
            mf.mesh.uv = wmb.UVs(TextureType.InnerFill);
            mf.mesh.RecalculateBounds();
            mf.mesh.RecalculateNormals();
        }

        void OnTriggerEnter2D(Collider2D collider)
        {
            Rigidbody2D rb2D = collider.GetComponent<Rigidbody2D>();
            if (rb2D != null)
            {
                Splash(collider.transform.position.x, rb2D.velocity.y * rb2D.mass / 40f);
            }

            PlaneController pc = collider.GetComponent<PlaneController>();
            if (pc != null)
                pc.OceanCrash();
        }
    }
}