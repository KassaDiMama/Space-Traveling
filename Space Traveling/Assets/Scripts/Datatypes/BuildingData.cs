using System.Collections;
using System.Collections.Generic;
using UnityEngine;
class BuildingData
{
    public int x;
    public int y;
    public int width;
    public int height;
    public string type;
    public BuildingData(Building building)
    {
        this.x = (int)building.lastGridPosition.x;
        this.y = (int)building.lastGridPosition.y;
        this.width = (int)building.width;
        this.height = (int)building.height;
        this.type = building.type;
    }
}