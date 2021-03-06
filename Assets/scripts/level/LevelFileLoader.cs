﻿using SimpleJSON;
using System.Collections.Generic;
using UnityEngine;
using level.data;
using level.behaviours;

namespace level
{

    public class LevelFileLoader
    {

        readonly string filename;
        private JSONNode jsonNode;

        public LevelFileLoader(string filename)
        {
            this.filename = filename;
        }

        public void LoadAndParse()
        {
            string jsonText = GetFileContent();
            if (jsonText != null)
            {
                jsonNode = JSON.Parse(jsonText);
            }
            else throw new System.ArgumentNullException();
        }

        private string GetFileContent()
        {
            TextAsset textAsset = Resources.Load(filename) as TextAsset;
            if (textAsset == null)
                throw new System.Exception("Resource does not exist");
            return System.Text.Encoding.ASCII.GetString(textAsset.bytes);
        }

        public string GetName()
        {
            if (jsonNode == null)
                LoadAndParse();

            return jsonNode["name"];
        }

        public string GetSubtitle()
        {
            if (jsonNode == null)
                LoadAndParse();

            return jsonNode["subtitle"];
        }

        public float GetLength()
        {
            if (jsonNode == null)
                LoadAndParse();

            return jsonNode["length"].AsFloat;
        }

        public float GetHeight()
        {
            if (jsonNode == null)
                LoadAndParse();

            return jsonNode["height"].AsFloat;
        }

        public Vector2 GetPlayerStart()
        {
            if (jsonNode == null)
                LoadAndParse();

            return jsonNode["playerStart"].AsVector2();
        }

        public float GetTargetTime()
        {
            if (jsonNode == null)
                LoadAndParse();

            return jsonNode["targetTime"].AsFloat;
        }


        public GeomData[] GetGeomData()
        {
            if (jsonNode == null)
                LoadAndParse();

            List<GeomData> geomDataList = new List<GeomData>();
            foreach (JSONNode node in jsonNode["geom"].AsArray)
            {
                geomDataList.Add(ParseGeomNode(node));
            }

            return geomDataList.ToArray();
        }

        private GeomData ParseGeomNode(JSONNode node)
        {
            string name = node["name"];
            GeomType type = node["type"].AsEnum<GeomType>();
            GeomPosition position = node["position"].AsEnum<GeomPosition>();
            Vector2[] points = node["points"].AsArrayVector2();
            int pivotStartPoints = node["pivotStartPoints"].AsInt;
            int pivotEndPoints = node["pivotEndPoints"].AsInt;

            return new GeomData(name, type, position, points,pivotStartPoints,pivotEndPoints);
        }

        public SpriteData[] GetSpriteData()
        {
            if (jsonNode == null)
                LoadAndParse();

            List<SpriteData> spriteDataList = new List<SpriteData>();
            foreach (JSONNode node in jsonNode["sprites"].AsArray)
            {
                spriteDataList.Add(ParseSpriteNode(node));
            }

            return spriteDataList.ToArray();
        }

        private SpriteData ParseSpriteNode(JSONNode node)
        {
            string name = node["name"];
            string filePath = node["filePath"];
            Vector2 position = node["position"].AsVector2();
            Vector2 scale = node["scale"].AsVector2();
            float rotation = node["rotation"].AsFloat;
            Vector2[] collision = node["collision"].AsArrayVector2();

            return new SpriteData(name, filePath, position, scale, rotation, collision);
        }


        public PickupData[] GetPickupData()
        {
            if (jsonNode == null)
                LoadAndParse();

            List<PickupData> pickupDataList = new List<PickupData>();
            foreach (JSONNode node in jsonNode["pickups"].AsArray)
            {
                pickupDataList.Add(ParsePickupNode(node));
            }

            return pickupDataList.ToArray();
        }

        private PickupData ParsePickupNode(JSONNode node)
        {
            string name = node["name"];
            PickupType type = node["type"].AsEnum<PickupType>();
            Vector2 position = node["position"].AsVector2();
            float rotation = node["rotation"].AsFloat;

            return new PickupData(name, type, position, rotation);
        }

        public HoopData[] GetHoopData()
        {
            if (jsonNode == null)
                LoadAndParse();

            List<HoopData> hoopDataList = new List<HoopData>();
            foreach (JSONNode node in jsonNode["hoops"].AsArray)
            {
                hoopDataList.Add(ParseHoopNode(node));
            }

            return hoopDataList.ToArray();
        }

        private HoopData ParseHoopNode(JSONNode node)
        {
            string name = node["name"];
            Vector2 position = node["position"].AsVector2();
            float rotation = node["rotation"].AsFloat;
            Vector2 scale = node["scale"].AsVector2();

            return new HoopData(name, position, rotation, scale);
        }


        public EnvironmentData GetEnvironmentData()
        {
            if (jsonNode == null)
                LoadAndParse();

            JSONNode node = jsonNode["environment"];
            float wind = node["wind"].AsFloat;
            float shadowAngle = node["wind"].AsFloat;
            LevelPalate palate = node["palate"].AsEnum<LevelPalate>();
            int cloudCount = node["cloudCount"].AsInt;

            return new EnvironmentData(wind, shadowAngle, palate, cloudCount);
        }

    }

    public static class JSONExtensions
    {

        public static Vector2 AsVector2(this JSONNode node)
        {
            return new Vector2(node[0].AsFloat, node[1].AsFloat);
        }

        public static T AsEnum<T>(this JSONNode node)
        {
            return ParseEnum<T>(node);
        }

        public static T ParseEnum<T>(string value)
        {
            return (T)System.Enum.Parse(typeof(T), value, true);
        }

        public static Vector2[] AsArrayVector2(this JSONNode node)
        {
            Vector2[] array = new Vector2[node.Count];
            for (int i = 0; i < node.Count; i++)
            {
                array[i] = node[i].AsVector2();
            }
            return array;
        }

    }
}