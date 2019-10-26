using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
class BaseInformationMessage : Message
{
    public string baseData;
    //public BaseData baseData;
    public override void onReceive()
    {
        PlayerPrefs.SetString("baseData", baseData);
        RequestInventoryMessage inventoryMessage = new RequestInventoryMessage();
        networkManager.sendMessage(inventoryMessage.Serialize());
    }
}