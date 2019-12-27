using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RocketItemUI : MonoBehaviour
{
    public Button sendButton;
    public string prefabName;
    public Main main;
    // Start is called before the first frame update
    void Start()
    {
        sendButton.onClick.AddListener(onSend);
    }

    // Update is called once per frame
    void Update()
    {

    }
    void onSend()
    {
        //main.placeRocket(prefabName);
        main.sendRocket(prefabName);
        main.inventoryUI.refreshUI();
    }
}
