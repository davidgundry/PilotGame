using SimpleJSON;
using level;
using System.Collections.Generic;
using UnityEngine;

public class LevelFileLoader {

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
        return System.Text.Encoding.ASCII.GetString(textAsset.bytes);
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
        GeomType type = node["type"].AsEnum<GeomType>();
        GeomPosition position = node["position"].AsEnum<GeomPosition>();
        Vector2[] points = node["points"].AsArrayVector2();

        return new GeomData(type, position, points);
    }

    public SpriteData[] GetSpriteData()
    {
        if (jsonNode == null)
            LoadAndParse();

        List<SpriteData> spriteDataList = new List<SpriteData>();
        foreach (JSONNode node in jsonNode["sprite"].AsArray)
        {
            spriteDataList.Add(ParseSpriteNode(node));
        }

        return spriteDataList.ToArray();
    }

    private SpriteData ParseSpriteNode(JSONNode node)
    {
        string filePath = node["filePath"];
        Vector2 position = node["position"].AsVector2();
        Vector2 scale = node["scale"].AsVector2();
        float rotation = node["rotation"].AsFloat;
        Vector2[] collision = node["collision"].AsArrayVector2();

        return new SpriteData(filePath, position, scale, rotation, collision);
    }

    public EnvironmentData GetEnvironmentData()
    {
        if (jsonNode == null)
            LoadAndParse();

        JSONNode node = jsonNode["environment"];
        float wind = node["wind"].AsFloat;
        float shadowAngle = node["wind"].AsFloat;
        LevelPalate palate = node["palate"].AsEnum<LevelPalate>();

        return new EnvironmentData(wind, shadowAngle, palate);
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
        for (int i=0;i<node.Count;i++)
        {
            array[i] = node[i].AsVector2();
        }
        return array;
    }

}