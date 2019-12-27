using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Main : MonoBehaviour
{
    // Start is called before the first frame update
    [HideInInspector]
    public Inventory inventory;
    [HideInInspector]
    public IsometricGrid grid;
    [HideInInspector]
    public Building currentlyEditing;
    [HideInInspector]
    public Building currentlySelected;
    public InventoryUI inventoryUI;

    public Transform gridParent;

    public CenterPanel centerPanel;
    [HideInInspector]
    public bool editing = false;
    [HideInInspector]
    private NetworkManager networkManager;
    public bool viewOnly = false;

    void Start()
    {
        inventory = Inventory.Deserialize(PlayerPrefs.GetString("inventoryData"));

        networkManager = GameObject.Find("NetworkManager").GetComponent<NetworkManager>();
        GameObject gridSprite = (GameObject)Resources.Load("Prefabs/Grid");
        float width = gridSprite.GetComponent<Renderer>().bounds.size.x;
        float widthWithEdgesMerged = width * 0.95f;
        if (viewOnly)
        {
            grid = IsometricGrid.Deserialize(PlayerPrefs.GetString("friendBaseData"));
            grid.viewOnly = true;
        }
        else
        {
            grid = IsometricGrid.Deserialize(PlayerPrefs.GetString("baseData"));
            grid.viewOnly = false;
        }

        grid.buildingPlaced.AddListener(OnBuildingPlaced);
        grid.placeGrid(gridParent);
        // if (!centerPanel.inventoryPanelUp)
        // {
        //     hideGrid();
        // }


        //grid.placeBuilding((GameObject)Resources.Load("Prefabs/Ground3x2"), new Vector3(3, 3, 0));
        //grid.placeBuilding((GameObject)Resources.Load("Prefabs/Ground3x2"), new Vector3(4, 0, 0));
        BaseInformationMessage baseInformation = new BaseInformationMessage();
        baseInformation.baseData = grid.Serialize();
        // networkManager.sendMessage(baseInformation.Serialize());
        //grid.placeBuilding((GameObject)Resources.Load("Prefabs/Ground3x2"));
        // inventory.addItem("Ground3x1", 10);
        // Debug.Log(inventory.Serialize());
        //inventoryUI.refreshUI();
        if (PlayerPrefs.GetString("rocketDestination") != null && PlayerPrefs.GetString("rocketDestination") != "")
        {
            //Destination destination = Destination.Deserialize(PlayerPrefs.GetString("rocketDestination"));
            placeRocket(PlayerPrefs.GetString("currentRocketType"), PlayerPrefs.GetString("rocketDestination"));
            PlayerPrefs.SetString("currentRocketType", "");
            PlayerPrefs.SetString("rocketDestination", "");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (currentlyEditing == null)
            {
                if (currentlySelected != null)
                {
                    if (currentlySelected.mouseDown == false)
                    {
                        currentlySelected.stopSelected();
                    }
                }
            }
        }
    }
    public void OnBuildingPlaced(Building building)
    {
        building.onMouseDown.AddListener(delegate { OnBuildingMouseDown(building); });
        building.isEditing.AddListener(delegate { onIsEditing(building.gameObject); });
        building.completeEditing.AddListener(delegate { onCompleteEditing(building.gameObject); });
        building.stopEditing.AddListener(delegate { onStopEditing(building.gameObject); });
        building.removeBuilding.AddListener(delegate { onRemoveBuilding(building.gameObject); });
        building.hasRotated.AddListener(delegate { onRotatedBuilding(building.gameObject); });


    }

    public void onIsEditing(GameObject buildingGameObject)
    {
        if (viewOnly)
        {
            Building building = buildingGameObject.GetComponent<Building>();
            building.stopEditMode();
        }
        else
        {
            if (currentlyEditing == null)
            {
                Building building = buildingGameObject.GetComponent<Building>();
                currentlyEditing = building;
                building.editing = true;
                Camera.main.GetComponent<CameraScript>().canMove = false;

            }
            else
            {
                onCompleteEditing(currentlyEditing.gameObject);
                Building building = buildingGameObject.GetComponent<Building>();
                currentlyEditing = building;
                building.editing = true;
                Camera.main.GetComponent<CameraScript>().canMove = false;
            }
            showGrid();
        }


        //Debug.Log("ohnono");
    }
    public void onCompleteEditing(GameObject buildingGameObject)
    {
        Building building = buildingGameObject.GetComponent<Building>();
        Camera.main.GetComponent<CameraScript>().canMove = true;
        currentlyEditing = null;
        if (!grid.isOnBoard(building) || grid.isOnBuilding(building))
        {
            if (building.lastGridPosition.x != -1f)
            {
                if (building.rotated)
                {
                    building.OnRotation();
                    building.rotated = false;
                }
                building.gridPosition = building.lastGridPosition;
                grid.changePositionOfBuilding(building.gameObject);
                currentlyEditing = null;
                Camera.main.GetComponent<CameraScript>().canMove = true;
                building.hideEditButtons();
                building.editing = false;

            }
            else
            {

                Camera.main.GetComponent<CameraScript>().canMove = true;
                currentlyEditing = null;
                Destroy(buildingGameObject);
                inventory.addItem(buildingGameObject.name.Replace("(Clone)", ""), "Building");
                inventoryUI.refreshUI();
                grid.removeBuilding(building);
            }

        }
        else
        {
            ChangeBuildingPositionMessage msg = new ChangeBuildingPositionMessage();
            msg.fromX = (int)building.lastGridPosition.x;
            msg.fromY = (int)building.lastGridPosition.y;
            msg.toX = (int)building.gridPosition.x;
            msg.toY = (int)building.gridPosition.y;
            msg.toWidth = (int)building.width;
            msg.toHeight = (int)building.height;
            msg.buildingType = building.type;
            networkManager.sendMessage(msg);
            building.lastGridPosition = building.gridPosition;

            grid.changePositionOfBuilding(building.gameObject);
            currentlyEditing = null;
            Camera.main.GetComponent<CameraScript>().canMove = true;
            building.hideEditButtons();
            building.editing = false;
        }
        if (!centerPanel.inventoryPanelUp)
        {
            hideGrid();
        }

    }
    public void onStopEditing(GameObject buildingGameObject)
    {
        Building building = buildingGameObject.GetComponent<Building>();
        if (building.lastGridPosition.x == -1f)
        {
            Camera.main.GetComponent<CameraScript>().canMove = true;
            currentlyEditing = null;
            Destroy(buildingGameObject);
            inventory.addItem(buildingGameObject.name.Replace("(Clone)", ""), "Building");
            inventoryUI.refreshUI();
            grid.removeBuilding(building);
        }
        else
        {
            if (building.rotated)
            {
                building.OnRotation();
                building.rotated = false;
            }
            building.gridPosition = building.lastGridPosition;
            grid.changePositionOfBuilding(building.gameObject);
            currentlyEditing = null;
            Camera.main.GetComponent<CameraScript>().canMove = true;
        }
        if (!centerPanel.inventoryPanelUp)
        {
            hideGrid();
        }




    }

    public void OnBuildingMouseDown(Building building)
    {
        //Debug.Log(building.gameObject.name);
        if (currentlySelected == null)
        {
            currentlySelected = building;
            building.startSelected();
        }
        else
        {
            currentlySelected.stopSelected();
            currentlySelected = building;
            building.startSelected();
        }
        if (viewOnly)
        {
            building.hideSelectedMenu();
        }
    }
    public void onRemoveBuilding(GameObject buildingGameObject)
    {
        Camera.main.GetComponent<CameraScript>().canMove = true;
        currentlyEditing = null;
        Destroy(buildingGameObject);
        inventory.addItem(buildingGameObject.name.Replace("(Clone)", ""), "Building");
        inventoryUI.refreshUI();
        grid.removeBuilding(buildingGameObject.GetComponent<Building>());
        if (!centerPanel.inventoryPanelUp)
        {
            hideGrid();
        }
        RemoveBuildingMessage RBM = new RemoveBuildingMessage();
        RBM.buildingData = new BuildingData(buildingGameObject.GetComponent<Building>());
        networkManager.sendMessage(RBM);
    }
    public void onRotatedBuilding(GameObject buildingGameObject)
    {
        grid.changePositionOfBuilding(buildingGameObject);
    }
    public void showGrid()
    {
        editing = true;
        gridParent.localScale = new Vector3(1, 1, 1);
    }
    public void hideGrid()
    {
        editing = false;
        gridParent.localScale = new Vector3(0, 0, 0);
    }
    public void placeRocket(string prefabName, string destination, bool needsKey = true)
    {
        List<RocketHolder> rocketHolderList = grid.getAllEmptyRocketHolders();
        if (rocketHolderList.Count > 0)
        {
            Rocket rocket = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/" + prefabName)).GetComponent<Rocket>();
            string key = grid.getRocketKey();
            rocket.key = key;
            Debug.Log("key just made: " + rocket.key);
            rocket.destination = destination;
            RocketHolder rocketHolder = rocketHolderList[0];
            if (rocketHolder != null)
            {
                rocketHolder.addRocket(rocket);
                Debug.Log(rocketHolder.rocket);
                AddRocketOnBuildingMessage message = new AddRocketOnBuildingMessage();
                message.buildingX = (int)rocketHolder.building.lastGridPosition.x;
                message.buildingY = (int)rocketHolder.building.lastGridPosition.y;
                message.destination = destination;
                message.type = rocket.type;
                message.rocketKey = key;
                networkManager.sendMessage(message);
                inventory.removeItem(rocket.type);
                if (!viewOnly)
                {
                    PlayerPrefs.SetString("baseData", grid.Serialize());
                }

                Debug.Log(grid.Serialize());
            }


        }
        else
        {
            Debug.Log("No open rocketholders");
        }
    }
    public void sendRocket(string type)
    {
        PlayerPrefs.SetString("currentRocketType", type);
        SceneManager.LoadScene("PlanetMap");
    }
}
