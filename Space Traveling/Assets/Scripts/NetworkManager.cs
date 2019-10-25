using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Net.Sockets;

public class NetworkManager : MonoBehaviour
{
    private TCP tcp = new TCP();
    public UnityEvent onServerConnect = new UnityEvent();
    public UnityEvent onServerFailedToConnect = new UnityEvent();
    //public UnityMainThreadDispatcher UMTD;
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        //DontDestroyOnLoad(UMTD.gameObject);
        tcp.messageReceived.AddListener(onMessageReceived);
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(onServerConnect.GetPersistentEventCount());
        if (tcp.isConnected())
        {
            tcp.readMessages();
        }

    }

    public void Connect(string host, int port)
    {
        tcp.Connect(host, port, new AsyncCallback(ConnectComplete));
    }
    private void ConnectComplete(IAsyncResult result)
    {
        try
        {

            if (tcp.isConnected())
            {
                this.onServerConnect.Invoke();
            }
            else
            {
                onServerFailedToConnect.Invoke();
            }
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }

    }
    public void ping()
    {
        Debug.Log("Ping");
    }

    public void sendMessage(string message)
    {
        tcp.sendMessage(message);
    }
    private void onMessageReceived(string message)
    {
        var messageObject = Message.Deserialize(message);
        messageObject.onReceive();
    }
}
