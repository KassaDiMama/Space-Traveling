using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
class AddRocketOnBuildingMessage : Message
{
    public int buildingX;
    public int buildingY;
    public string type;
    public string destination;
    public string rocketKey;

    //public BaseData baseData;

}
