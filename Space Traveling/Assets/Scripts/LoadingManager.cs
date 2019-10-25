using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;


public class LoadingManager : MonoBehaviour
{
    private NetworkManager networkManager;
    private string host = "217.122.147.106";
    private int port = 33333;
    // Start is called before the first frame update
    void Start()
    {
        networkManager = GameObject.Find("NetworkManager").GetComponent<NetworkManager>();
        networkManager.onServerConnect.AddListener(handleOnServerConnect);
        networkManager.onServerFailedToConnect.AddListener(handleOnServerFailedToConnect);

        networkManager.Connect(host, port);

    }

    // Update is called once per frame
    void Update()
    {

    }

    void handleOnServerConnect()
    {
        Debug.Log("connected");
        UnityMainThreadDispatcher.Instance().Enqueue(loadNextScene());
        // RequestBase requestBase = new RequestBase();
        // requestBase.username = "Kassa";
        // networkManager.sendMessage(requestBase.Serialize());

    }
    void handleOnServerFailedToConnect()
    {
        //SceneManager.LoadScene("GameMap");
        Debug.Log("connected");
    }
    IEnumerator loadNextScene()
    {
        yield return new WaitForSeconds(0.1f);
        SceneManager.LoadScene("GameMap");
    }
}
