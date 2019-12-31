using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

using TMPro;



public class InventoryUI : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject buildingSample;
    public GameObject rocketSample;
    public GameObject friendSample;
    public Main main;
    public RectTransform buildingContent;
    public RectTransform buildingContentViewPort;
    public RectTransform rocketContent;
    public RectTransform rocketContentViewPort;
    public RectTransform friendsContent;
    public RocketsUI rocketsUI;
    private NetworkManager networkManager;

    void Start()
    {
        //main.inventory.addItem("Ground3x2",1);
        networkManager = GameObject.Find("NetworkManager").GetComponent<NetworkManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void refreshUI()
    {
        FriendsList friendsList = FriendsList.Deserialize(PlayerPrefs.GetString("friendsList"));

        foreach (Transform child in buildingContent)
        {
            Destroy(child.gameObject);
        }

        foreach (Transform child in friendsContent)
        {
            Destroy(child.gameObject);
        }
        rocketsUI.refreshUI();
        foreach (InventoryItem item in main.inventory.inventoryList)
        {
            if (item.type == "Building")
            {
                GameObject itemPrefab = (GameObject)Resources.Load("Prefabs/" + item.prefabName);
                GameObject newItem = GameObject.Instantiate(buildingSample);
                //Debug.Log(itemPrefab);


                //newItem.GetComponent<EventTrigger>().triggers.PointerDown.AddListener(delegate{itemDown(newItem);});
                //newItem.GetComponent<EventTrigger>().PointerUp.AddListener(delegate{itemUp(newItem);});

                newItem.transform.SetParent(buildingContent, false);
                newItem.transform.localScale = Vector3.one;
                newItem.GetComponent<InventoryItemUI>().contentTransform = buildingContent;
                newItem.GetComponent<InventoryItemUI>().inventoryTransform = gameObject.GetComponent<RectTransform>();
                newItem.GetComponent<InventoryItemUI>().placedOnGrid.AddListener(delegate { onPlaceOnGrid(item, newItem); });

                newItem.GetComponent<InventoryItemUI>().prefabName = item.prefabName;
                newItem.transform.Find("Amount").gameObject.GetComponent<TMP_Text>().text = item.amount.ToString();

                //LayoutRebuilder.ForceRebuildLayoutImmediate(buildingContent);
                newItem.transform.Find("Image").GetComponent<Image>().sprite = itemPrefab.GetComponent<Building>().buildingRenderer.sprite;

            }
            else if (item.type == "Rocket")
            {


            }
            foreach (Friend friend in friendsList.friends)
            {
                GameObject newFriend = GameObject.Instantiate(friendSample);
                newFriend.transform.Find("FriendName").gameObject.GetComponent<TMP_Text>().text = friend.username;
                newFriend.transform.SetParent(friendsContent, false);
                newFriend.transform.localScale = Vector3.one;
                newFriend.transform.Find("ViewBaseButton").gameObject.GetComponent<Button>().onClick.AddListener(delegate { onViewFriendBase(friend); });
            }
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
    public void show()
    {
        GetComponent<CanvasGroup>().alpha = 1f;
    }
    public void hide()
    {
        GetComponent<CanvasGroup>().alpha = 0f;
    }

    public void onViewFriendBase(Friend friend)
    {
        RequestFriendsBaseMessage message = new RequestFriendsBaseMessage();
        message.friendName = friend.username;
        networkManager.sendMessage(message);
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
