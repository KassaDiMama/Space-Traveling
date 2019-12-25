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
        if (GameObject.Find("NetworkManager") != null)
        {
            networkManager = GameObject.Find("NetworkManager").GetComponent<NetworkManager>();
        }
        else
        {
            GameObject networkManagerInstance = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/NetworkManager"));
            networkManagerInstance.name = "NetworkManager";
            networkManager = networkManagerInstance.GetComponent<NetworkManager>();
        }
        networkManager.onServerConnect.AddListener(handleOnServerConnect);
        networkManager.onServerFailedToConnect.AddListener(handleOnServerFailedToConnect);
        if (networkManager.key != "")
        {
            Debug.Log("WHAAAAAAAAAAAAAT");
            Debug.Log(networkManager.key);
            RequestBaseMessage requestBase = new RequestBaseMessage();
            //requestBase.username = "Kassa";
            networkManager.sendMessage(requestBase);
        }
        else
        {
            Debug.Log(networkManager.key);
            networkManager.Connect(host, port);
        }


    }

    // Update is called once per frame
    void Update()
    {

    }

    void handleOnServerConnect()
    {
        Debug.Log("connected");
        UnityMainThreadDispatcher.Instance().Enqueue(connectedFunction());







    }
    IEnumerator connectedFunction()
    {
        yield return new WaitForSeconds(0.1f);
        Debug.Log(PlayerPrefs.GetString("Username") == "");
        if (PlayerPrefs.GetString("Username") != "" && PlayerPrefs.GetString("Password") != "")
        {
            LoginMessage loginMessage = new LoginMessage();
            loginMessage.username = PlayerPrefs.GetString("Username");
            loginMessage.password = PlayerPrefs.GetString("Password");
            networkManager.sendMessage(loginMessage);
        }
        else
        {
            SceneManager.LoadScene("LoginScene");
        }

    }
    void handleOnServerFailedToConnect()
    {
        //SceneManager.LoadScene("GameMap");
        Debug.Log("Failed to connect..");
    }

}
