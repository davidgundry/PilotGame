using UnityEngine;
using System.Collections;
using level.data;
using player;
using player.behaviour;
using player.data;
using level.behaviours;

namespace level
{
    public class LevelBehaviour : MonoBehaviour
    {
        GeomBehaviour[] geomBehaviours;
        SpriteBehaviour[] spriteBehaviours;
        CloudBehaviour[] cloudBehaviours;
        PickupBehaviour[] pickupBehaviours;
        HoopBehaviour[]  hoopBehaviours;
        LevelBounds levelBounds;
        PlaneController player;
        FollowCamera followCamera;
        private bool created;
        LevelSession levelSession;

        void Update()
        {
            if (created)
            {
                CheckLevelBounds();
                if (player.HasCrashed())
                {
                    if (player.InOcean)
                        levelSession.PlayerSunk();
                    else
                        levelSession.PlayerCrashed();
                }
            }
        }

        private void CheckLevelBounds()
        {
            player.transform.position = levelBounds.ApplyHardHeightLimit(player.transform.position);
            levelBounds.ApplySoftHeightLimit(player.transform.position, player.GetComponent<Rigidbody2D>(), Time.deltaTime);
            if (levelBounds.CheckLevelComplete(player.transform.position))
            {
                player.AutoPilot = true;
                levelSession.CrossedFinishLine();
            }
            if (player.transform.position.y < levelBounds.GeomBottomEdge)
            {
                levelSession.FallOffBottomOfScreen();
            }
        }



        public void Create(LevelSession levelSession, LevelData levelData, PlayerLevelData playerLevelData)
        {
            this.levelSession = levelSession;
            player = CreatePlayer(playerLevelData);
            followCamera = CreateCamera(player);
            levelBounds = CreateLevelBounds(levelData, followCamera);
            followCamera.SetCameraBounds(levelBounds);

            geomBehaviours = CreateGeom(levelData.geomData, levelBounds);
            spriteBehaviours  = CreateSprites(levelData.spriteData);
            pickupBehaviours = CreatePickups(levelData.pickupData);
            hoopBehaviours = CreateHoops(levelData.hoopData);

            CreateFinishLine(levelData,levelBounds);
            CreateCloudLine(levelData);
            cloudBehaviours = CreateClouds(levelBounds, levelData.environmentData);

            SetPlayerPosition(levelData);
            created = true;
        }

        private static GeomBehaviour[] CreateGeom(GeomData[] geomData, LevelBounds levelBounds)
        {
            GeomBehaviour[] geomBehaviours = new GeomBehaviour[geomData.Length];
            GameObject geomParent = new GameObject();
            geomParent.name = "geom";
            for (int i = 0; i < geomData.Length; i++)
            {
                GameObject g = new GameObject();
                if (geomData[i].geomType == GeomType.Ocean)
                {
                    g.transform.parent = geomParent.transform;
                    g.AddComponent<WaterBehaviour>();
                    g.GetComponent<WaterBehaviour>().Create(geomData[i], levelBounds);
                    geomBehaviours[i] = g.GetComponent<WaterBehaviour>();
                }
                else
                {
                    g.transform.parent = geomParent.transform;
                    g.AddComponent<GeomBehaviour>();
                    g.GetComponent<GeomBehaviour>().Create(geomData[i], levelBounds);
                    geomBehaviours[i] = g.GetComponent<GeomBehaviour>();
                }
                g.name = geomData[i].name;
            }
            return geomBehaviours;
        }

        private static SpriteBehaviour[] CreateSprites(SpriteData[] spriteData)
        {
            SpriteBehaviour[] spriteBehaviours = new SpriteBehaviour[spriteData.Length];
            GameObject spriteParent = new GameObject();
            spriteParent.name = "sprites";
            for (int i = 0; i < spriteData.Length; i++)
            {
                GameObject g = new GameObject();
                g.transform.parent = spriteParent.transform;
                g.AddComponent<SpriteBehaviour>();
                g.GetComponent<SpriteBehaviour>().Create(spriteData[i]);
                g.name = spriteData[i].name;
                spriteBehaviours[i] = g.GetComponent<SpriteBehaviour>();
            }
            return spriteBehaviours;
        }

        private PickupBehaviour[] CreatePickups(PickupData[] pickupData)
        {
            PickupBehaviour[] pickupBehaviours = new PickupBehaviour[pickupData.Length];
            GameObject pickupParent = new GameObject();
            pickupParent.name = "pickups";
            for (int i = 0; i < pickupData.Length; i++)
            {
                if (pickupData[i].pickupType == PickupType.Coin)
                {
                    GameObject g = Instantiate(levelSession.CoinPrefab);
                    g.transform.SetParent(pickupParent.transform, false);
                    g.transform.position = pickupData[i].position;
                    g.name = pickupData[i].name;
                }
                else
                {
                    GameObject g = new GameObject();
                    g.transform.parent = pickupParent.transform;
                    g.AddComponent<PickupBehaviour>();
                    g.GetComponent<PickupBehaviour>().Create(pickupData[i]);
                    g.name = pickupData[i].name;
                    pickupBehaviours[i] = g.GetComponent<PickupBehaviour>();
                }
            }
            return pickupBehaviours;
        }

        private HoopBehaviour[] CreateHoops(HoopData[] hoopData)
        {
            HoopBehaviour[] hoopBehaviours = new HoopBehaviour[hoopData.Length];
            GameObject hoopParent = new GameObject();
            hoopParent.name = "hoops";
            GameObject hoop = levelSession.HoopPrefab;
            for (int i = 0; i < hoopData.Length; i++)
            {
                GameObject g = Instantiate(hoop);
                g.transform.parent = hoopParent.transform;
                g.GetComponentInChildren<HoopBehaviour>().Create(hoopData[i]);
                g.name = hoopData[i].name;
                hoopBehaviours[i] = g.GetComponentInChildren<HoopBehaviour>();
            }
            return hoopBehaviours;
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

        private PlaneController CreatePlayer(PlayerLevelData playerLevelData)
        {
            GameObject plane = Instantiate(levelSession.PlayerPrefab);
            PlaneController planeController = plane.GetComponent<PlaneController>();
            planeController.PlayerLevelData = playerLevelData;
            return planeController;
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

        private static CloudBehaviour[] CreateClouds(LevelBounds levelBounds,EnvironmentData environmentData)
        {
            int cloudCount = environmentData.cloudCount;
            CloudBehaviour[] cloudBehaviours = new CloudBehaviour[cloudCount];
            for (int i = 0; i < cloudBehaviours.Length; i++)
            {
                GameObject g = new GameObject();
                g.AddComponent<CloudBehaviour>();
                cloudBehaviours[i] = g.GetComponent<CloudBehaviour>();
                cloudBehaviours[i].Create(levelBounds, environmentData);
            }
            return cloudBehaviours;
        }

        private void SetPlayerPosition(LevelData levelData)
        {
            float playerZ = -2;
            player.transform.position = new Vector3(levelData.playerStart.x,levelData.playerStart.y,playerZ);
        }

        public void FreezePlay(bool frozen)
        {
            if (frozen)
            {
                player.Freeze();
                player.enabled = false;
                foreach (TrailRenderer trail in player.transform.GetComponentsInChildren<TrailRenderer>())
                {
                        trail.enabled = false;
                }
                foreach (CloudBehaviour cloud in cloudBehaviours)
                {
                    cloud.enabled = false;
                }
                foreach (GeomBehaviour geom in geomBehaviours)
                {
                    geom.enabled = false;
                }
            }
            else
            {
                player.Unfreeze();
                player.enabled = true;
                foreach (TrailRenderer trail in player.transform.GetComponentsInChildren<TrailRenderer>())
                {
                    trail.enabled = true;
                }
                foreach (CloudBehaviour cloud in cloudBehaviours)
                {
                    cloud.enabled = true;
                }
                foreach (GeomBehaviour geom in geomBehaviours)
                {
                    geom.enabled = true;
                }
            }
        }
    }
}