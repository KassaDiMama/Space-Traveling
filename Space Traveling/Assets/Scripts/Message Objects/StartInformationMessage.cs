using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
class StartInformationMessage : Message
{
    public string baseData;
    public string inventoryData;
    public string friendsList;
    public override void onReceive()
    {
        PlayerPrefs.SetString("inventoryData", inventoryData);
        PlayerPrefs.SetString("baseData", baseData);
        PlayerPrefs.SetString("friendsList", friendsList);
        UnityMainThreadDispatcher.Instance().Enqueue(loadNextScene());
    }
    IEnumerator loadNextScene()
    {
        yield return new WaitForSeconds(0.1f);
        SceneManager.LoadScene("GameMap");
    }
}
