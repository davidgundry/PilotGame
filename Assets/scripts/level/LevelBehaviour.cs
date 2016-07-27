﻿using UnityEngine;
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

            CreateFinishLine(levelData,levelBounds);
            CreateCloudLine(levelData);
            cloudBehaviours = CreateClouds(levelBounds, levelData.environmentData);

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

        private static PickupBehaviour[] CreatePickups(PickupData[] pickupData)
        {
            PickupBehaviour[] pickupBehaviours = new PickupBehaviour[pickupData.Length];
            for (int i = 0; i < pickupData.Length; i++)
            {
                GameObject g = new GameObject();
                g.AddComponent<PickupBehaviour>();
                g.GetComponent<PickupBehaviour>().Create(pickupData[i]);
                g.name = pickupData[i].name;
                pickupBehaviours[i] = g.GetComponent<PickupBehaviour>();
            }
            return pickupBehaviours;
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

        private static PlaneController CreatePlayer(PlayerLevelData playerLevelData)
        {
            GameObject plane = Instantiate(Resources.Load<GameObject>("prefabs/player"));
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
            player.transform.position = levelData.playerStart;
        }

        public void FreezePlay(bool frozen)
        {
            if (frozen)
            {
                player.Freeze();
                foreach (CloudBehaviour cloud in cloudBehaviours)
                {
                    cloud.enabled = false;
                }
            }
            else
            {
                player.Unfreeze();
                foreach (CloudBehaviour cloud in cloudBehaviours)
                {
                    cloud.enabled = true;
                }
            }
        }
    }
}