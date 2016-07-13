using UnityEngine;
using System.Collections;
using level.data;
using player;

namespace level
{
    public class LevelBehaviour : MonoBehaviour
    {
        GeomBehaviour[] geomBehaviours;
        SpriteBehaviour[] spriteBehaviours;
        LevelBounds levelBounds;
        PlaneController player;
        FollowCamera camera;
        private bool created;

        void Update()
        {
            if (created)
            {
                CheckLevelBounds();
            }
        }

        private void CheckLevelBounds()
        {
            player.transform.position = levelBounds.ApplyHardHeightLimit(player.transform.position);
            levelBounds.ApplySoftHeightLimit(player.transform.position, player.GetComponent<Rigidbody2D>(), Time.deltaTime);
            if (levelBounds.CheckLevelComplete(player.transform.position))
            {
                // Level is complete
            }
        }

        public void Create(LevelData levelData)
        {
            player = CreatePlayer();
            camera = CreateCamera(player);
            levelBounds = CreateLevelBounds(levelData, camera);
            camera.SetCameraBounds(levelBounds);

            geomBehaviours = CreateGeom(levelData.geomData, levelBounds);
            spriteBehaviours  = CreateSprites(levelData.spriteData);

            CreateFinishLine(levelData,levelBounds);
            CreateCloudLine(levelData);
            CreateClouds(levelBounds,levelData.environmentData);

            SetPlayerPosition(levelData);
            created = true;
        }

        private static GeomBehaviour[] CreateGeom(GeomData[] geomData, LevelBounds levelBounds)
        {
            GeomBehaviour[] geomBehaviours = new GeomBehaviour[geomData.Length];
            for (int i = 0; i < geomData.Length; i++)
            {
                GameObject g = new GameObject();
                g.AddComponent<GeomBehaviour>();
                g.GetComponent<GeomBehaviour>().Create(geomData[i],levelBounds);
                g.name = geomData[i].name;
                geomBehaviours[i] = g.GetComponent<GeomBehaviour>();
            }
            return geomBehaviours;
        }

        private static SpriteBehaviour[] CreateSprites(SpriteData[] spriteData)
        {
            SpriteBehaviour[] spriteBehaviours = new SpriteBehaviour[spriteData.Length];
            for (int i = 0; i < spriteData.Length; i++)
            {
                GameObject g = new GameObject();
                g.AddComponent<SpriteBehaviour>();
                g.GetComponent<SpriteBehaviour>().Create(spriteData[i]);
                g.name = spriteData[i].name;
                spriteBehaviours[i] = g.GetComponent<SpriteBehaviour>();
            }
            return spriteBehaviours;
        }

        private static void CreateFinishLine(LevelData levelData,LevelBounds levelBounds)
        {
            GameObject finishLine = new GameObject();
            finishLine.name = "FinishLine";
            finishLine.AddComponent<FinishLineBehaviour>();
            finishLine.GetComponent<FinishLineBehaviour>().Create(levelData,levelBounds);
        }

        private static void CreateCloudLine(LevelData levelData)
        {
            float zPosition = 1;
            GameObject cloudLine = new GameObject();
            cloudLine.name = "CloudLine";
            cloudLine.AddComponent<CloudLineBehaviour>();
            cloudLine.GetComponent<CloudLineBehaviour>().Create(levelData, zPosition);
        }

        private static PlaneController CreatePlayer()
        {
            GameObject plane = Instantiate(Resources.Load<GameObject>("prefabs/player"));
            return plane.GetComponent<PlaneController>();
        }

        private static FollowCamera CreateCamera(PlaneController player)
        {
            GameObject g = new GameObject();
            g.AddComponent<FollowCamera>();
            FollowCamera camera = g.GetComponent<FollowCamera>();
            camera.Create(player.transform);
            return camera;
        }

        private static LevelBounds CreateLevelBounds(LevelData levelData, FollowCamera camera)
        {
            return new LevelBounds(levelData.length, 0, 0, levelData.height, levelData.height+0.5f, camera.Camera.orthographicSize, camera.Camera.orthographicSize*camera.Camera.aspect);
        }

        private static void CreateClouds(LevelBounds levelBounds,EnvironmentData environmentData)
        {
            int cloudCount = 10;
            for (int i = 0; i < cloudCount; i++)
            {
                GameObject g = new GameObject();
                g.AddComponent<CloudBehaviour>();
                CloudBehaviour cloud = g.GetComponent<CloudBehaviour>();
                cloud.Create(levelBounds,environmentData);
            }
        }

        private void SetPlayerPosition(LevelData levelData)
        {
            player.transform.position = levelData.playerStart;
        }
    }
}