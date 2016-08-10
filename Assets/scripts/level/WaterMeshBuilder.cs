using UnityEngine;
using System.Collections;
using level.data;
namespace level
{
    public class WaterMeshBuilder : GeomMeshBuilder
    {

        public WaterMeshBuilder(GeomData geomData, LevelBounds levelBounds) : base(geomData, levelBounds)
        {
        }

    }
}
