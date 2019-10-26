using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

using TMPro;



public class InventoryUI : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject sample;
    public Main main;
    public RectTransform content;
    public RectTransform contentViewPort;

    void Start()
    {
        //main.inventory.addItem("Ground3x2",1);
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void refreshUI()
    {

        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
        }
        foreach (InventoryItem item in main.inventory.inventoryList)
        {

            GameObject itemPrefab = (GameObject)Resources.Load("Prefabs/" + item.prefabName);
            GameObject newItem = GameObject.Instantiate(sample);
            //Debug.Log(itemPrefab);


            //newItem.GetComponent<EventTrigger>().triggers.PointerDown.AddListener(delegate{itemDown(newItem);});
            //newItem.GetComponent<EventTrigger>().PointerUp.AddListener(delegate{itemUp(newItem);});

            newItem.transform.SetParent(content, false);
            newItem.transform.localScale = Vector3.one;
            newItem.GetComponent<InventoryItemUI>().contentTransform = content;
            newItem.GetComponent<InventoryItemUI>().inventoryTransform = gameObject.GetComponent<RectTransform>();
            newItem.GetComponent<InventoryItemUI>().placedOnGrid.AddListener(delegate { onPlaceOnGrid(item, newItem); });

            newItem.GetComponent<InventoryItemUI>().prefabName = item.prefabName;
            newItem.transform.Find("Amount").gameObject.GetComponent<TMP_Text>().text = item.amount.ToString();

            //LayoutRebuilder.ForceRebuildLayoutImmediate(content);
            newItem.transform.Find("Image").GetComponent<Image>().sprite = itemPrefab.GetComponent<Building>().buildingRenderer.sprite;
        }
    }

    public void onPlaceOnGrid(InventoryItem item, GameObject itemUI)
    {
        GameObject itemPrefab = (GameObject)Resources.Load("Prefabs/" + item.prefabName);
        //itemPrefab.GetComponent<Building>().mouseDown = true;

        GameObject itemGameObject = main.grid.placeBuilding(itemPrefab, new Vector3(-1, -1, -1));
        //itemGameObject.GetComponent<Building>().isEditing.AddListener(delegate{onIsEditing(itemGameObject);});
        //itemGameObject.GetComponent<Building>().completeEditing.AddListener(delegate{onCompleteEditing(itemGameObject);});
        //itemGameObject.GetComponent<Building>().stopEditing.AddListener(delegate{onStopEditing(itemGameObject);});
        itemGameObject.GetComponent<Building>().startEditing();
        itemGameObject.GetComponent<Building>().mouseDown = true;
        main.inventory.removeItem(itemGameObject.name.Replace("(Clone)", ""));
        Destroy(itemUI);
        refreshUI();
    }
    /*
    public void onIsEditing(GameObject buildingGameObject){
        if(main.currentlyEditing == null){
            Building building = buildingGameObject.GetComponent<Building>();
            main.currentlyEditing = building;
            building.editing=true;
            Camera.main.GetComponent<CameraScript>().canMove=false;
            
        }
    }
    public void onCompleteEditing(GameObject buildingGameObject){
        Building building = buildingGameObject.GetComponent<Building>();
        Camera.main.GetComponent<CameraScript>().canMove=true;
        main.currentlyEditing = null;
        if(main.grid.isOnBuilding(building) || !main.grid.canMoveTo(building)){
            Destroy(buildingGameObject);
            main.inventory.addItem(buildingGameObject.name.Replace("(Clone)",""));
            refreshUI();
            main.grid.removeBuilding(building);
            
        }
        
    }
    public void onStopEditing(GameObject buildingGameObject){
        Building building = buildingGameObject.GetComponent<Building>();
        Camera.main.GetComponent<CameraScript>().canMove=true;
        main.currentlyEditing = null;
        Destroy(buildingGameObject);
        main.inventory.addItem(buildingGameObject.name.Replace("(Clone)",""));
        refreshUI();
        main.grid.removeBuilding(building);
        

        
    }*/
}
