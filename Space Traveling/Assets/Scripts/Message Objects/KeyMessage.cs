using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
class KeyMessage : Message
{
    public string key;
    //public BaseData baseData;
    public override void onReceive()
    {
        NetworkManager networkManager = GameObject.Find("NetworkManager").GetComponent<NetworkManager>();
        networkManager.key = this.key;
        Debug.Log(this.key);
        UnityMainThreadDispatcher.Instance().Enqueue(loadNextScene());

        // IsometricGrid newGrid = IsometricGrid.Deserialize(baseData);
        // Main main = GameObject.Find("Main").GetComponent<Main>();
        // main.updateGrid()
    }
    IEnumerator loadNextScene()
    {
        yield return new WaitForSeconds(0.1f);
        SceneManager.LoadScene("LoadingScene");
    }
}