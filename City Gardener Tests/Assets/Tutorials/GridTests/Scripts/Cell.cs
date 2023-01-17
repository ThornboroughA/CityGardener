using System.Threading;
using UnityEngine;

[System.Serializable]
public class Cell
{
    public enum TerrainType { field, forest, marsh, water }

    public TerrainType terrainType;
    public bool isWater;

    public Vector2 cellCoordinates;

    public Person pop;

    public Cell()
    {
        if (!isWater)
            this.pop = new Person();
    }

    public TerrainType AssignTerrainType(float threshold)
    {
        TerrainType terrain = TerrainType.water;

        if (threshold < 0)
        {
            Debug.LogWarning("Threshold value of " + threshold + " is below accepted parameters.");
        } else if (threshold <= 0.3)
        {
            terrain = TerrainType.field;
        } else if ( threshold <= 0.8)
        {
            terrain = TerrainType.forest;
        } else
        {
            terrain = TerrainType.marsh;
        }

        return terrain;
    }
    
}
