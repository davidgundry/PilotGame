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
            CreatePlayer();
            CreateCamera(player);
            CreateLevelBounds(levelData, camera);
            camera.SetCameraBounds(levelBounds);

            CreateGeom(levelData.geomData, levelBounds);
            CreateSprites(levelData.spriteData);

            CreateFinishLine(levelData);
            CreateCloudLine(levelData);

            SetPlayerPosition(levelData);
            created = true;
        }

        private void CreateGeom(GeomData[] geomData, LevelBounds levelBounds)
        {
            geomBehaviours = new GeomBehaviour[geomData.Length];
            for (int i = 0; i < geomData.Length; i++)
            {
                GameObject g = new GameObject();
                g.AddComponent<GeomBehaviour>();
                g.GetComponent<GeomBehaviour>().Create(geomData[i],levelBounds);
                g.name = geomData[i].name;
                geomBehaviours[i] = g.GetComponent<GeomBehaviour>();
            }
        }

        private void CreateSprites(SpriteData[] spriteData)
        {
            spriteBehaviours = new SpriteBehaviour[spriteData.Length];
            for (int i = 0; i < spriteData.Length; i++)
            {
                GameObject g = new GameObject();
                g.AddComponent<SpriteBehaviour>();
                g.GetComponent<SpriteBehaviour>().Create(spriteData[i]);
                g.name = spriteData[i].name;
                spriteBehaviours[i] = g.GetComponent<SpriteBehaviour>();
            }
        }

        private void CreateFinishLine(LevelData levelData)
        {
            GameObject finishLine = new GameObject();
            finishLine.name = "FinishLine";
            finishLine.AddComponent<FinishLineBehaviour>();
            finishLine.GetComponent<FinishLineBehaviour>().Create(levelData);
        }

        private void CreateCloudLine(LevelData levelData)
        {
            float zPosition = 1;
            GameObject cloudLine = new GameObject();
            cloudLine.name = "CloudLine";
            cloudLine.AddComponent<CloudLineBehaviour>();
            cloudLine.GetComponent<CloudLineBehaviour>().Create(levelData, zPosition);
        }

        private void CreatePlayer()
        {
            GameObject plane = Instantiate(Resources.Load<GameObject>("prefabs/player"));
            player = plane.GetComponent<PlaneController>();
        }

        private void CreateCamera(PlaneController player)
        {
            GameObject g = new GameObject();
            g.AddComponent<FollowCamera>();
            camera = g.GetComponent<FollowCamera>();
            camera.Create(player.transform);
        }

        private void CreateLevelBounds(LevelData levelData, FollowCamera camera)
        {
            levelBounds = new LevelBounds(levelData.length, 0, 0, levelData.height, levelData.height+0.5f, camera.Camera.orthographicSize, camera.Camera.orthographicSize*camera.Camera.aspect);
        }

        private void SetPlayerPosition(LevelData levelData)
        {
            player.transform.position = levelData.playerStart;
        }
    }
}