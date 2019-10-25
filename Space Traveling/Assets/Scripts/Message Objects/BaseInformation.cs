using System.Collections;
using System.Collections.Generic;
using UnityEngine;
class BaseInformation : Message
{
    public string baseData;
    //public BaseData baseData;
    public override void onReceive()
    {
        Debug.Log("WOW HAAHA LOOK AT THIS KRAZEEEEY baseData: " + baseData);
    }
}