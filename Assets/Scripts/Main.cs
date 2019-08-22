using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    // Start is called before the first frame update
    public Inventory inventory = new Inventory();
    public IsometricGrid grid;
    public Building currentlyEditing;
    public Building currentlySelected;
    public InventoryUI inventoryUI;
    void Start()
    {
        GameObject gridSprite = (GameObject)Resources.Load("Prefabs/Grid");
        float width = gridSprite.GetComponent<Renderer>().bounds.size.x;
        float widthWithEdgesMerged = width*0.95f;
        grid = new IsometricGrid(10,widthWithEdgesMerged);
        grid.buildingPlaced.AddListener(OnBuildingPlaced);
        grid.placeGrid();
        
        grid.placeBuilding((GameObject)Resources.Load("Prefabs/Ground3x2"),new Vector3(0,3,0));
        //grid.placeBuilding((GameObject)Resources.Load("Prefabs/Ground3x2"));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnBuildingPlaced(Building building){
        building.onMouseDown.AddListener(delegate{OnBuildingMouseDown(building);});
        building.isEditing.AddListener(delegate{onIsEditing(building.gameObject);});
        building.completeEditing.AddListener(delegate{onCompleteEditing(building.gameObject);});
        building.stopEditing.AddListener(delegate{onStopEditing(building.gameObject);});
        

    }
    
    public void onIsEditing(GameObject buildingGameObject){
        if(currentlyEditing == null){
            Building building = buildingGameObject.GetComponent<Building>();
            currentlyEditing = building;
            building.editing=true;
            Camera.main.GetComponent<CameraScript>().canMove=false;
            
        }
    }
    public void onCompleteEditing(GameObject buildingGameObject){
        Building building = buildingGameObject.GetComponent<Building>();
        Camera.main.GetComponent<CameraScript>().canMove=true;
        currentlyEditing = null;
        if(grid.isOnBuilding(building) || !grid.canMoveTo(building)){
            Destroy(buildingGameObject);
            inventory.addItem(buildingGameObject.name.Replace("(Clone)",""));
            inventoryUI.refreshUI();
            grid.removeBuilding(building);
            
        }
        
    }
    public void onStopEditing(GameObject buildingGameObject){
        Building building = buildingGameObject.GetComponent<Building>();
        Camera.main.GetComponent<CameraScript>().canMove=true;
        currentlyEditing = null;
        Destroy(buildingGameObject);
        inventory.addItem(buildingGameObject.name.Replace("(Clone)",""));
        inventoryUI.refreshUI();
        grid.removeBuilding(building);
        

        
    }
    
    public void OnBuildingMouseDown(Building building){
        Debug.Log("Down building");
        if(currentlySelected == null){
            currentlySelected = building;
            building.startSelected();
        }else{
            currentlySelected.stopSelected();
            currentlySelected = building;
            building.startSelected();
        }
    }
}
