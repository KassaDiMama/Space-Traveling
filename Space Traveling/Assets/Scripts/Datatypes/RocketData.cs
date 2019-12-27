using System.Collections;
using System.Collections.Generic;
using UnityEngine;
class RocketData
{
    public string key;
    public string destination;
    public string type;
    public RocketData(Rocket rocket)
    {
        this.key = rocket.key;
        this.destination = rocket.destination;
        this.type = rocket.type;
    }
}