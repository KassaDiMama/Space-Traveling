using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
class Message
{
    public string command = "Message";
    public NetworkManager networkManager;
    public string key;
    public Message()
    {
        command = this.GetType().Name;
    }

    public string Serialize()
    {
        return JsonConvert.SerializeObject(this, Formatting.Indented, new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore
        });
        //return JsonConvert.SerializeObject(this);
    }
    public static dynamic Deserialize(string jsonString)
    {
        Debug.Log(jsonString);
        JObject dict = JObject.Parse(jsonString);
        Type objectType = Type.GetType(dict["command"].Value<string>());
        Debug.Log(dict["command"].Value<string>());
        var instantiatedObject = JsonConvert.DeserializeObject(jsonString, objectType);
        return instantiatedObject;
    }

    public virtual void onReceive()
    {
        Debug.Log("Haaha i just received this crazy message: " + command);
    }


}
