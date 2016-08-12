using UnityEngine;
using System.Collections.Generic;
using level.data;
using player.behaviour;

namespace level.behaviours
{
    public struct WaterNode
    {
        public readonly float x, y, velocity, acceleration;
        public WaterNode(float x, float y, float velocity, float acceleration)
        {
            this.x = x;
            this.y = y;
            this.velocity = velocity;
            this.acceleration = acceleration;
        }

        public WaterNode SetVelocity(float velocity)
        {
            return new WaterNode(this.x, this.y, velocity, this.acceleration);
        }

        public WaterNode ApplyForce(float force)
        {
            float newAcceleration = force;
            float newY = this.y + this.velocity;
            float newVelocity = this.velocity + acceleration;
            float newX = this.x;


            return new WaterNode(newX, newY, newVelocity, newAcceleration);
        }
    }

    [RequireComponent(typeof(MeshFilter))]
    public class WaterBehaviour : GeomBehaviour
    {
        private WaterNode[] nodes;
        private LineRenderer lineRenderer;

        private Mesh mesh;
        private int quads;        

        const float springConstant = 0.02f;
        const float damping = 0.06f;
        const float spread = 0.09f;
        const float z = -1f;

        float baseheight;
        float left;
        float bottom;
        private int NodeCount { get; set; }

        private ParticleSystem particleSystem;
        private Material lineMaterial;

        private void CreateLine()
        {
            lineRenderer = gameObject.AddComponent<LineRenderer>();
            lineRenderer.material = lineMaterial;
            lineRenderer.material.renderQueue = 1000;
            lineRenderer.SetVertexCount(NodeCount);
            lineRenderer.SetWidth(0.7f, 0.7f);
        }

        public void SpawnWater(float left, float width, float baseheight, float bottom)
        {
            this.baseheight = baseheight;
            this.bottom = bottom;
            this.left = left;

            int edgecount = Mathf.RoundToInt(width)*5;
            quads = edgecount;
            NodeCount = edgecount + 1;

            CreateLine();
            InitialiseNodes(width);
            CreateMesh();
            UpdateLinePositions();
        }

        private void InitialiseNodes(float width)
        {
            nodes = new WaterNode[NodeCount];

            for (int i = 0; i < NodeCount; i++)
            {
                nodes[i] = new WaterNode(left + width * i / quads, baseheight, 0, 0);
            }
        }

        private void CreateMesh()
        {
            mesh = new Mesh();
            mesh.SetVertices(CreateMeshVertices());
            mesh.SetUVs(0, CreateMeshUVs());
            mesh.SetTriangles(CreateMeshTris(), 0);
            GetComponent<MeshFilter>().mesh = mesh;
        }

        private List<Vector3> CreateMeshVertices()
        {
            List<Vector3> vertices = new List<Vector3>();
            for (int i = 0; i < quads; i++)
            {
                vertices.Add(new Vector3(nodes[i].x, nodes[i].y, z));
                vertices.Add(new Vector3(nodes[i+1].x, nodes[i + 1].y, z));
                vertices.Add(new Vector3(nodes[i].x, bottom, z));
                vertices.Add(new Vector3(nodes[i + 1].x, bottom, z));
            }
            return vertices;
        }

        private List<Vector2> CreateMeshUVs()
        {
            List<Vector2> uvs = new List<Vector2>();
            for (int i = 0; i < quads; i++)
            {
                uvs.Add(new Vector2(0.25f, 0.45f));
                uvs.Add(new Vector2(0.25f, 0.45f));
                uvs.Add(new Vector2(0.25f, 0.45f));
                uvs.Add(new Vector2(0.25f, 0.45f));
            }
            return uvs;
        }

        private List<int> CreateMeshTris()
        {
            List<int> tris = new List<int>();
            for (int i = 0; i < quads; i++)
            {
                int trisInset = i * 4;
                tris.AddRange(new int[6] { trisInset + 0, trisInset + 1, trisInset + 3, trisInset + 3, trisInset + 2, trisInset + 0 });
            }
            return tris;
        }

        private void UpdateMeshPositions()
        {
            mesh.SetVertices(CreateMeshVertices());
        }

        private void ApplySpringPhysics()
        {
            for (int i=0;i<NodeCount;i++)
            {
                float force = springConstant * (nodes[i].y - baseheight) + nodes[i].velocity * damping;
                nodes[i] = nodes[i].ApplyForce(-force);
            }
        }

        private void UpdateNodes(float[] deltaPositions, float[] deltaVelocities)
        {
            int index = 0;
            foreach (WaterNode node in nodes)
            {
                nodes[index] = new WaterNode(node.x, node.y + deltaPositions[index], node.velocity + deltaVelocities[index], node.acceleration);
                index++;
            }
        }

        private void ApplySpreadAndWaves()
        {
            float[] deltaPositions = new float[NodeCount];
            float[] deltaVelocities = new float[NodeCount];

            for (int i = 0; i < NodeCount; i++)
            {
                if (i > 0)
                {
                    float leftDelta = spread * (nodes[i].y - nodes[i - 1].y);
                    deltaVelocities[i - 1] += leftDelta;
                    deltaPositions[i - 1] += leftDelta;
                }
                if (i < NodeCount - 1)
                {
                    float rightDelta = spread * (nodes[i].y - nodes[i + 1].y);
                    deltaVelocities[i + 1] += rightDelta;
                    deltaPositions[i + 1] += rightDelta;
                }
               deltaVelocities[i] += (Random.value - 0.5f) * 0.01f; // Waves
            }

            UpdateNodes(deltaPositions,deltaVelocities);
        }

        private void UpdateLinePositions()
        {
            int index = 0;
            foreach (WaterNode node in nodes)
            {
                lineRenderer.SetPosition(index, new Vector3(node.x, node.y, z));
                index++;
            }
        }

        void FixedUpdate()
        {
            ApplySpringPhysics();
            ApplySpreadAndWaves();
            UpdateMeshPositions();
            UpdateLinePositions();
        }


        public void Splash(float xPosition, float velocity)
        {
            if ((xPosition >= nodes[0].x) && (xPosition <= nodes[NodeCount - 1].x))
            {
                xPosition -= nodes[0].x;
                int index = Mathf.RoundToInt((NodeCount - 1) * (xPosition / (nodes[NodeCount - 1].x - nodes[0].x)));
                nodes[index] = nodes[index].SetVelocity(velocity);

                //particleSystem.startSpeed = 8 + 8 * Mathf.Pow(Mathf.Abs(velocity), 0.5f);
                Vector3 position = new Vector3(nodes[index].x, nodes[index].y - 0.35f, 8);
                Quaternion rotation = Quaternion.LookRotation(new Vector3(nodes[Mathf.FloorToInt(NodeCount / 2)].x, baseheight + 8, 5) - position);
                ParticleSystem particles = Instantiate(particleSystem);
                particles.transform.position = position;
                Destroy(particles, 2f);
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
            MakeCollider(geomData, levelBounds);

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