using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
class AcceptFriendMessage : Message
{
    public string friendsList;
    public override void onReceive()
    {
        PlayerPrefs.SetString("friendsList", friendsList);
        Debug.Log(friendsList);
    }

}
