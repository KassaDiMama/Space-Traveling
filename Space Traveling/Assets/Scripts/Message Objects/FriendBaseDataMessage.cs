using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
class FriendBaseDataMessage : Message
{
    public string baseData;
    public override void onReceive()
    {
        PlayerPrefs.SetString("friendBaseData", baseData);
        UnityMainThreadDispatcher.Instance().Enqueue(loadNextScene());


    }
    IEnumerator loadNextScene()
    {
        yield return new WaitForSeconds(0.1f);
        SceneManager.LoadScene("BaseViewer");
    }


}
