using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
class ChangeBuildingPositionMessage : Message
{
    public string username;
    public int fromX;
    public int fromY;
    public int toX;
    public int toY;
    public int toWidth;
    public int toHeight;
    public string buildingType;

    //public BaseData baseData;

}
