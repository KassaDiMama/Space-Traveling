using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Net.Sockets;

[System.Serializable]
public class messageEventTCP : UnityEvent<string>
{
}

public class TCP
{
    private TcpClient client;
    public messageEventTCP messageReceived = new messageEventTCP();


    public void Connect(string ip, int port, AsyncCallback callback)
    {
        client = new TcpClient();
        client.BeginConnect(ip, port, callback, "");

    }

    public void sendMessage(string message)
    {
        if (client.Connected)
        {
            Byte[] data = System.Text.Encoding.ASCII.GetBytes(message);

            NetworkStream stream = client.GetStream();

            stream.Write(data, 0, data.Length);

        }
    }
    void readMessages()
    {
        if (client != null)
        {
            List<string> responses = new List<string>();
            NetworkStream stream = client.GetStream();
            while (client.Available >= 1)
            {
                Byte[] data = new Byte[256];

                // Read the first batch of the TcpServer response bytes.
                Int32 bytes = stream.Read(data, 0, data.Length);
                string responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
                messageReceived.Invoke(responseData);
            }
        }
    }
    public bool isConnected()
    {
        return client.Connected;
    }

}