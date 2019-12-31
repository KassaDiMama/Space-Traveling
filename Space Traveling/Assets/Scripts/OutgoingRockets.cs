using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
class OutgoingRockets
{
    public List<RocketTransaction> transactions = new List<RocketTransaction>();
    public OutgoingRockets()
    {

    }
    public void addTransaction(RocketTransaction transaction)
    {
        transactions.Add(transaction);
    }
    public string Serialize()
    {

        return JsonConvert.SerializeObject(this);
        //return JsonConvert.SerializeObject(this);

    }
    public static OutgoingRockets Deserialize(string jsonString)
    {
        OutgoingRockets outgoingRockets = new OutgoingRockets();
        Debug.Log(jsonString);
        JObject dict = JObject.Parse(jsonString);
        foreach (var transaction in dict["transactions"])
        {
            string newRocketType = transaction["rocketType"].Value<string>();
            Debug.Log(newRocketType);
            string newSenderName = transaction["senderName"].Value<string>();
            Destination newDestination = Destination.Deserialize(transaction["destination"].Value<string>());
            string newTime = transaction["time"].Value<string>();
            RocketTransaction rocketTransaction = new RocketTransaction(newRocketType, newSenderName, newDestination, newTime);
            outgoingRockets.addTransaction(rocketTransaction);
        }
        return outgoingRockets;
    }
}