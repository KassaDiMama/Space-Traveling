using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    // Start is called before the first frame update
    public Inventory inventory;
    public IsometricGrid grid;
    public Building currentlyEditing;
    public Building currentlySelected;
    public InventoryUI inventoryUI;
    public Transform gridParent;
    public CenterPanel centerPanel;
    public bool editing = false;
    private NetworkManager networkManager;
    void Start()
    {
        inventory = Inventory.Deserialize(PlayerPrefs.GetString("inventoryData"));
        Debug.Log(inventory);
        networkManager = GameObject.Find("NetworkManager").GetComponent<NetworkManager>();
        GameObject gridSprite = (GameObject)Resources.Load("Prefabs/Grid");
        float width = gridSprite.GetComponent<Renderer>().bounds.size.x;
        float widthWithEdgesMerged = width * 0.95f;
        grid = IsometricGrid.Deserialize(PlayerPrefs.GetString("baseData"));
        grid.buildingPlaced.AddListener(OnBuildingPlaced);
        grid.placeGrid(gridParent);
        if (!centerPanel.inventoryUp)
        {
            hideGrid();
        }


        //grid.placeBuilding((GameObject)Resources.Load("Prefabs/Ground3x2"), new Vector3(3, 3, 0));
        //grid.placeBuilding((GameObject)Resources.Load("Prefabs/Ground3x2"), new Vector3(4, 0, 0));
        BaseInformationMessage baseInformation = new BaseInformationMessage();
        baseInformation.baseData = grid.Serialize();
        // networkManager.sendMessage(baseInformation.Serialize());
        //grid.placeBuilding((GameObject)Resources.Load("Prefabs/Ground3x2"));
        // inventory.addItem("Ground3x1", 10);
        // Debug.Log(inventory.Serialize());
        inventoryUI.refreshUI();
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
                inventory.addItem(buildingGameObject.name.Replace("(Clone)", ""));
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
            msg.username = "Kassa";
            msg.buildingType = building.type;
            networkManager.sendMessage(msg.Serialize());
            building.lastGridPosition = building.gridPosition;

            grid.changePositionOfBuilding(building.gameObject);
            currentlyEditing = null;
            Camera.main.GetComponent<CameraScript>().canMove = true;
            building.hideEditButtons();
            building.editing = false;
        }
        if (!centerPanel.inventoryUp)
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
            inventory.addItem(buildingGameObject.name.Replace("(Clone)", ""));
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
        if (!centerPanel.inventoryUp)
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
    }
    public void onRemoveBuilding(GameObject buildingGameObject)
    {
        Camera.main.GetComponent<CameraScript>().canMove = true;
        currentlyEditing = null;
        Destroy(buildingGameObject);
        inventory.addItem(buildingGameObject.name.Replace("(Clone)", ""));
        inventoryUI.refreshUI();
        grid.removeBuilding(buildingGameObject.GetComponent<Building>());
        if (!centerPanel.inventoryUp)
        {
            hideGrid();
        }
        RemoveBuildingMessage RBM = new RemoveBuildingMessage();
        RBM.buildingData = new BuildingData(buildingGameObject.GetComponent<Building>());
        networkManager.sendMessage(RBM.Serialize());
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
}
