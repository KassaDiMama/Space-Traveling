using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    // Start is called before the first frame update
    public string type;
    private RocketHolder rocketHolder;
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void placeOnHolder(RocketHolder rocketHolder)
    {
        this.rocketHolder = rocketHolder;
        transform.position = rocketHolder.getRocketPosition();
        GetComponent<SpriteRenderer>().sortingOrder = rocketHolder.GetComponent<SpriteRenderer>().sortingOrder + 1;
    }
}
