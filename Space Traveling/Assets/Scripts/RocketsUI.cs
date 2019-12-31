using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

using TMPro;



public class RocketsUI : MonoBehaviour
{
    // Start is called before the first frame update
    public RectTransform content;
    public GameObject sample;
    public GameObject incomingSample;
    public Button rocketInventoryButton;
    public Button incomingRocketsButton;
    private bool showInventoryRockets = true;
    private Main main;


    void Start()
    {
        //main.inventory.addItem("Ground3x2",1);
        incomingRocketsButton.onClick.AddListener(delegate { showInventoryRockets = false; refreshUI(); });
        rocketInventoryButton.onClick.AddListener(delegate { showInventoryRockets = true; refreshUI(); });
        main = GameObject.Find("Main").GetComponent<Main>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void show()
    {
        GetComponent<CanvasGroup>().alpha = 1f;
        GetComponent<CanvasGroup>().blocksRaycasts = true;
    }
    public void hide()
    {
        GetComponent<CanvasGroup>().alpha = 0f;
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }
    public void refreshUI()
    {
        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
        }
        if (showInventoryRockets)
        {
            foreach (InventoryItem item in main.inventory.inventoryList)
            {
                if (item.type == "Rocket")
                {

                    GameObject itemPrefab = (GameObject)Resources.Load("Prefabs/" + item.prefabName);
                    GameObject newItem = GameObject.Instantiate(sample);
                    RocketItemUI rocketItemUI = newItem.GetComponent<RocketItemUI>();
                    rocketItemUI.prefabName = item.prefabName;
                    rocketItemUI.main = main;

                    //Debug.Log(itemPrefab);


                    //newItem.GetComponent<EventTrigger>().triggers.PointerDown.AddListener(delegate{itemDown(newItem);});
                    //newItem.GetComponent<EventTrigger>().PointerUp.AddListener(delegate{itemUp(newItem);});
                    newItem.transform.SetParent(content, false);
                    newItem.transform.localScale = Vector3.one;
                    newItem.transform.Find("AmountPanel").Find("Text").GetComponent<TMP_Text>().text = item.amount.ToString();
                    //LayoutRebuilder.ForceRebuildLayoutImmediate(buildingContent);
                    newItem.transform.Find("Image").GetComponent<Image>().sprite = itemPrefab.GetComponent<SpriteRenderer>().sprite;

                }
            }
        }
        else
        {
            OutgoingRockets outgoingRockets = OutgoingRockets.Deserialize(PlayerPrefs.GetString("outgoingRockets"));
            foreach (RocketTransaction transaction in outgoingRockets.transactions)
            {
                Debug.Log(transaction.rocketType);
                GameObject itemPrefab = (GameObject)Resources.Load("Prefabs/" + transaction.rocketType);
                GameObject newItem = GameObject.Instantiate(incomingSample);
                RocketItemUI rocketItemUI = newItem.GetComponent<RocketItemUI>();
                rocketItemUI.prefabName = transaction.rocketType;
                rocketItemUI.main = main;

                //Debug.Log(itemPrefab);


                //newItem.GetComponent<EventTrigger>().triggers.PointerDown.AddListener(delegate{itemDown(newItem);});
                //newItem.GetComponent<EventTrigger>().PointerUp.AddListener(delegate{itemUp(newItem);});
                newItem.transform.SetParent(content, false);
                newItem.transform.localScale = Vector3.one;
                //LayoutRebuilder.ForceRebuildLayoutImmediate(buildingContent);
                newItem.transform.Find("Image").GetComponent<Image>().sprite = itemPrefab.GetComponent<SpriteRenderer>().sprite;
            }
        }

    }

}


