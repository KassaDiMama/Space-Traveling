using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
class InventoryInformationMessage : Message
{
    public string inventoryData;
    //public BaseData baseData;
    public override void onReceive()
    {

        PlayerPrefs.SetString("inventoryData", inventoryData);
        Debug.Log(inventoryData);
        UnityMainThreadDispatcher.Instance().Enqueue(loadNextScene());
        // IsometricGrid newGrid = IsometricGrid.Deserialize(baseData);
        // Main main = GameObject.Find("Main").GetComponent<Main>();
        // main.updateGrid()
    }
    IEnumerator loadNextScene()
    {
        yield return new WaitForSeconds(0.1f);
        SceneManager.LoadScene("GameMap");
    }
}