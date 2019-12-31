using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
class RocketTransaction
{
    public string rocketType;
    public Destination destination;
    public string senderName;
    public string time;
    public bool landed = false;
    public bool canLand = false;
    public RocketTransaction(string rocketType, string senderName, Destination destination, string time)
    {
        this.rocketType = rocketType;
        this.senderName = senderName;
        this.destination = destination;
        this.time = time;
        this.landed = false;
        this.canLand = false;

    }
    public string Serialize()
    {

        return JsonConvert.SerializeObject(this);
        //return JsonConvert.SerializeObject(this);

    }
    public static RocketTransaction Deserialize(string jsonString)
    {
        JObject dict = JObject.Parse(jsonString);
        string newRocketType = dict["rocketType"].Value<string>();
        string newSenderName = dict["senderName"].Value<string>();
        Destination newDestination = Destination.Deserialize(dict["destination"].Value<string>());
        string newTime = dict["time"].Value<string>();
        RocketTransaction rocketTransaction = new RocketTransaction(newRocketType, newSenderName, newDestination, newTime);
        return rocketTransaction;
    }
}