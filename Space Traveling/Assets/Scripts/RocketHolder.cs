using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketHolder : MonoBehaviour
{
    // Start is called before the first frame update
    public Building building;
    public Rocket rocket;
    private NetworkManager networkManager;
    private Main main;
    public void Start()
    {
        //building = GetComponent<Building>();
        networkManager = GameObject.Find("NetworkManager").GetComponent<NetworkManager>();
        main = GameObject.Find("Main").GetComponent<Main>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void addRocket(Rocket rocket)
    {
        this.rocket = rocket;
        this.rocket.placeOnHolder(this);
    }
    public void removeRocket()
    {
        RemoveRocketOnBuildingMessage message = new RemoveRocketOnBuildingMessage();
        message.buildingX = (int)building.lastGridPosition.x;
        message.buildingY = (int)building.lastGridPosition.y;
        networkManager.sendMessage(message);
        main.inventory.addItem(rocket.type, "Rocket");
        main.inventoryUI.refreshUI();
        Destroy(rocket.gameObject);
    }

    public Vector3 getRocketPosition()
    {
        Vector3 rocketPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        return rocketPos;
    }
    public void addRocketButtonPressed()
    {

        if (this.rocket == null)
        {

            GameObject rocketPrefab = Resources.Load<GameObject>("Prefabs/Rocket1");
            Rocket rocketInstance = GameObject.Instantiate(rocketPrefab).GetComponent<Rocket>();
            AddRocketOnBuildingMessage message = new AddRocketOnBuildingMessage();
            message.buildingX = (int)building.lastGridPosition.x;
            message.buildingY = (int)building.lastGridPosition.y;
            message.type = rocketInstance.type;
            networkManager.sendMessage(message);
            addRocket(rocketInstance);
            Debug.Log("Added rocket");
        }
    }
    public void removeRocketButtonPressed()
    {

        if (this.rocket != null)
        {
            removeRocket();
            Debug.Log("Removed rocket");
        }
    }
    public void updateRocketPosition()
    {
        if (rocket != null)
        {
            rocket.placeOnHolder(this);
        }

    }
    public void sendRocket()
    {
        if (rocket != null)
        {
            string rocketKey = rocket.key;
            Debug.Log("RocketKey: " + rocket.key);
            SendRocketMessage message = new SendRocketMessage();
            message.rocketKey = rocketKey;
            networkManager.sendMessage(message);
            Destroy(rocket.gameObject);
            rocket = null;
        }

    }
}
