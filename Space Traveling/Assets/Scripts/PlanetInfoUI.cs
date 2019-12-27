using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlanetInfoUI : MonoBehaviour
{
    public TMP_Text planetNameText;
    public Image planetImage;
    public Button exitButton;
    public Planet currentPlanet;
    // Start is called before the first frame update
    void Start()
    {
        exitButton.onClick.AddListener(hide);
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void setInfo(Planet planet)
    {
        planetNameText.text = planet.name;
        planetImage.sprite = planet.sprite;
        currentPlanet = planet;
    }
    public void show()
    {
        transform.localScale = Vector3.one;
    }
    public void hide()
    {
        transform.localScale = Vector3.zero;
    }
}
