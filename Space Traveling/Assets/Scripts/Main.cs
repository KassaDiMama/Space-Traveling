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
    public Transform gridParent;
    public CenterPanel centerPanel;
    public bool editing = false;
    void Start()
    {
        GameObject gridSprite = (GameObject)Resources.Load("Prefabs/Grid");
        float width = gridSprite.GetComponent<Renderer>().bounds.size.x;
        float widthWithEdgesMerged = width*0.95f;
        grid = new IsometricGrid(10,widthWithEdgesMerged);
        grid.buildingPlaced.AddListener(OnBuildingPlaced);
        grid.placeGrid(gridParent);
        if(!centerPanel.inventoryUp){
            hideGrid();
        }
    
        
        grid.placeBuilding((GameObject)Resources.Load("Prefabs/Ground3x2"),new Vector3(0,3,0));
        //grid.placeBuilding((GameObject)Resources.Load("Prefabs/Ground3x2"));
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0)){
            if(currentlyEditing==null){
                if(currentlySelected!=null){
                    if(currentlySelected.mouseDown==false){
                        currentlySelected.stopSelected();
                    }
                }
            }
        }
    }
    public void OnBuildingPlaced(Building building){
        building.onMouseDown.AddListener(delegate{OnBuildingMouseDown(building);});
        building.isEditing.AddListener(delegate{onIsEditing(building.gameObject);});
        building.completeEditing.AddListener(delegate{onCompleteEditing(building.gameObject);});
        building.stopEditing.AddListener(delegate{onStopEditing(building.gameObject);});
        building.removeBuilding.AddListener(delegate{onRemoveBuilding(building.gameObject);});
        building.hasRotated.AddListener(delegate{onRotatedBuilding(building.gameObject);});
        

    }
    
    public void onIsEditing(GameObject buildingGameObject){
        if(currentlyEditing == null){
            Building building = buildingGameObject.GetComponent<Building>();
            currentlyEditing = building;
            building.editing=true;
            Camera.main.GetComponent<CameraScript>().canMove=false;
            
        }else{
            onCompleteEditing(currentlyEditing.gameObject);
            Building building = buildingGameObject.GetComponent<Building>();
            currentlyEditing = building;
            building.editing=true;
            Camera.main.GetComponent<CameraScript>().canMove=false;
        }
        showGrid();
        
        //Debug.Log("ohnono");
    }
    public void onCompleteEditing(GameObject buildingGameObject){
        Building building = buildingGameObject.GetComponent<Building>();
        Camera.main.GetComponent<CameraScript>().canMove=true;
        currentlyEditing = null;
        if(!grid.isOnBoard(building) || grid.isOnBuilding(building)){
            if(building.lastGridPosition.x != -1f){
                if(building.rotated){
                    building.OnRotation();
                    building.rotated=false;
                }
                building.gridPosition = building.lastGridPosition;
                grid.changePositionOfBuilding(building.gameObject);
                currentlyEditing = null;
                Camera.main.GetComponent<CameraScript>().canMove=true;
                building.hideEditButtons();
                building.editing=false;
                
            }else{
                
                Camera.main.GetComponent<CameraScript>().canMove=true;
                currentlyEditing = null;
                Destroy(buildingGameObject);
                inventory.addItem(buildingGameObject.name.Replace("(Clone)",""));
                inventoryUI.refreshUI();
                grid.removeBuilding(building);
            }
            
        }else{
            
            building.lastGridPosition = building.gridPosition;
            grid.changePositionOfBuilding(building.gameObject);
            currentlyEditing = null;
            Camera.main.GetComponent<CameraScript>().canMove=true;
            building.hideEditButtons();
            building.editing=false;
        }
        if(!centerPanel.inventoryUp){
            hideGrid();
        }
        
    }
    public void onStopEditing(GameObject buildingGameObject){
        Building building = buildingGameObject.GetComponent<Building>();
        if(building.lastGridPosition.x == -1f){
            Camera.main.GetComponent<CameraScript>().canMove=true;
            currentlyEditing = null;
            Destroy(buildingGameObject);
            inventory.addItem(buildingGameObject.name.Replace("(Clone)",""));
            inventoryUI.refreshUI();
            grid.removeBuilding(building);
        }else{
            if(building.rotated){
                building.OnRotation();
                building.rotated=false;
            }
            building.gridPosition = building.lastGridPosition;
            grid.changePositionOfBuilding(building.gameObject);
            currentlyEditing = null;
            Camera.main.GetComponent<CameraScript>().canMove=true;
        }
        if(!centerPanel.inventoryUp){
            hideGrid();
        }
        
        

        
    }
    
    public void OnBuildingMouseDown(Building building){
        //Debug.Log(building.gameObject.name);
        if(currentlySelected == null){
            currentlySelected = building;
            building.startSelected();
        }else{
            currentlySelected.stopSelected();
            currentlySelected = building;
            building.startSelected();
        }
    }
    public void onRemoveBuilding(GameObject buildingGameObject){
        Camera.main.GetComponent<CameraScript>().canMove=true;
        currentlyEditing = null;
        Destroy(buildingGameObject);
        inventory.addItem(buildingGameObject.name.Replace("(Clone)",""));
        inventoryUI.refreshUI();
        grid.removeBuilding(buildingGameObject.GetComponent<Building>());
        if(!centerPanel.inventoryUp){
            hideGrid();
        }
    }
    public void onRotatedBuilding(GameObject buildingGameObject){
        grid.changePositionOfBuilding(buildingGameObject);
    }
    public void showGrid(){
        editing=true;
        gridParent.localScale = new Vector3(1,1,1);
    }
    public void hideGrid(){
        editing=false;
        gridParent.localScale = new Vector3(0,0,0);
    }
}
