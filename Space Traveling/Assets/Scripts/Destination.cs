using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
class Destination
{
    public bool toPlanet = false;
    public bool toPlayer = false;
    public string planetName;
    public string playerName;

    public Destination()
    {

    }
    public string Serialize()
    {

        return JsonConvert.SerializeObject(this);
        //return JsonConvert.SerializeObject(this);

    }
    public static Destination Deserialize(string jsonString)
    {
        JObject dict = JObject.Parse(jsonString);
        Destination destination = JsonConvert.DeserializeObject<Destination>(jsonString);
        return destination;
    }
}