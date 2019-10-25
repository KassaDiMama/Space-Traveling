using System.Collections;
using System.Collections.Generic;
using UnityEngine;
class BaseData
{
    public int width;
    public int length;
    public List<BuildingData> buildings;
    public BaseData(IsometricGrid grid, List<Building> buildings)
    {
        this.width = (int)grid.width;
        this.length = (int)grid.length;
        List<BuildingData> serializedBuildings = new List<BuildingData>();
        foreach (Building building in buildings)
        {
            BuildingData buildingData = new BuildingData(building);
            serializedBuildings.Add(buildingData);
        }
        this.buildings = serializedBuildings;
    }
}