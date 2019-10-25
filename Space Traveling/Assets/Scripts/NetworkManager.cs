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
        onServerConnect.AddListener(delegate { ping(); });
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(onServerConnect.GetPersistentEventCount());
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
}
