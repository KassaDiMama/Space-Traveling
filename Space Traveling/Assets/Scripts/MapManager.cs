using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class MapManager : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform planets;
    public PlanetInfoUI planetInfoPanel;
    public Button sendButton;
    private NetworkManager networkManager;
    void Start()
    {
        sendButton.onClick.AddListener(sendRocket);
        foreach (Transform planet in planets)
        {
            Planet p = planet.gameObject.GetComponent<Planet>();
            planet.gameObject.GetComponent<GameObjectButton>().onMouseDown.AddListener(delegate { planetDown(p); });
        }
        networkManager = networkManager = GameObject.Find("NetworkManager").GetComponent<NetworkManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void planetDown(Planet planet)
    {
        planetInfoPanel.setInfo(planet);
        planetInfoPanel.show();
    }
    private void sendRocket()
    {
        Planet currentPlanet = planetInfoPanel.currentPlanet;
        string currentRocketType = PlayerPrefs.GetString("currentRocketType");
        Destination destination = new Destination();
        destination.toPlanet = true;
        destination.planetName = currentPlanet.name;
        PlayerPrefs.SetString("rocketDestination", destination.Serialize());
        // SendRocketToPlanetMessage message = new SendRocketToPlanetMessage();
        // message.planetName = currentPlanet.name;
        // message.rocketType = currentRocketType;
        // networkManager.sendMessage(message);
        SceneManager.LoadScene("GameMap");
    }
}
