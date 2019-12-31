using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RocketItemUI : MonoBehaviour
{

    public string prefabName;
    public Main main;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void onSend()
    {
        //main.placeRocket(prefabName);
        main.sendRocket(prefabName);
        main.inventoryUI.refreshUI();
    }
    public void onLand()
    {

    }
}
