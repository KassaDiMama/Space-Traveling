using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using TMPro;

[System.Serializable]
public class BuildingPlaced : UnityEvent<Building>
{
}

public class IsometricTile
{
    public int x;
    public int y;
    public Vector2 position;
    public GameObject building;
    public GameObject gridPicture;


    public IsometricTile(int x, int y, Vector2 position)
    {
        this.x = x;
        this.y = y;
        this.position = position;
    }
}

public class IsometricGrid
{
    private IsometricTile[,] grid;
    public Vector2 position;
    private float tileWidth;
    public int width;
    public int length;
    private List<Building> buildings = new List<Building>();
    public BuildingPlaced buildingPlaced = new BuildingPlaced();
    public JToken buildingsJson;

    public IsometricGrid(int width, int length, float tileWidth)
    {
        this.tileWidth = tileWidth;
        this.width = width;
        this.length = length;
        grid = new IsometricTile[width, length];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < length; y++)
            {
                Vector2 tilePosition = new Vector2();
                tilePosition.x = (x + y) * 0.5f * tileWidth;
                tilePosition.y = (x - y) * 0.25f * tileWidth;
                grid[x, y] = new IsometricTile(x, y, tilePosition);
                //Debug.Log(grid[x,y]==null);
            }
        }

    }
    public bool isOnBoard(Building building)
    {
        int x = (int)building.gridPosition.x;
        int y = (int)building.gridPosition.y;
        int sizeX = (int)building.width;
        int sizeY = (int)building.height;
        for (int checkX = x; checkX < x + sizeX; checkX++)
        {
            for (int checkY = y; checkY < y + sizeY; checkY++)
            {
                //Debug.Log(grid.GetLength(0)+" : "+checkX);
                if (checkX < 0 || grid.GetLength(0) <= checkX || 0 > checkY || checkY >= grid.GetLength(1))
                {
                    //Debug.Log("Not On Board, x: "+checkX+" y: "+checkY+" maxX : "+grid.GetLength(0)+" maxY: "+grid.GetLength(1));
                    return false;
                }
            }
        }
        return true;
    }
    public bool isOnBuilding(Building building)
    {
        int x = (int)building.gridPosition.x;
        int y = (int)building.gridPosition.y;
        int sizeX = (int)building.width;
        int sizeY = (int)building.height;
        for (int checkX = x; checkX < x + sizeX; checkX++)
        {
            for (int checkY = y; checkY < y + sizeY; checkY++)
            {
                if (grid[checkX, checkY].building != null && grid[checkX, checkY].building != building.gameObject)
                {
                    return true;
                }
            }
        }
        return false;
    }
    public void placeGrid(Transform parent)
    {
        for (int x = 0; x < grid.GetLength(0); x++)
        {
            for (int y = 0; y < grid.GetLength(1); y++)
            {
                GameObject textMesh = (GameObject)Resources.Load("Prefabs/Grid");
                //GameObject textMesh = (GameObject)Resources.Load("Prefabs/2DText");
                //textMesh.GetComponent<TMP_Text>().text = x+" , "+y;
                IsometricTile currentTile = grid[x, y];
                GameObject clone = GameObject.Instantiate(textMesh, new Vector3(currentTile.position.x, currentTile.position.y, 0), Quaternion.identity);
                currentTile.gridPicture = clone;
                clone.transform.SetParent(parent);
            }
        }
        if (buildingsJson != null)
        {
            foreach (var jsonBuilding in buildingsJson)
            {
                GameObject buildingGameObject = (GameObject)Resources.Load("Prefabs/" + jsonBuilding["type"].Value<string>());
                int x = jsonBuilding["x"].Value<int>();
                int y = jsonBuilding["y"].Value<int>();
                Vector3 buildingGridPosition = new Vector3(x, y, 0);
                placeBuilding(buildingGameObject, buildingGridPosition);
            }
        }
    }
    public GameObject placeBuilding(GameObject buildingGameObject, Vector3 gridPosition)
    {
        /*
        Building building = buildingGameObject.GetComponent<Building>();
        Vector3 buildingGridPosition = building.gridPosition+building.offset;
        Vector3 buildingPosition= new Vector3();
        buildingPosition.x = (buildingGridPosition.x+buildingGridPosition.y)* 0.5f * tileWidth;
        buildingPosition.y = (buildingGridPosition.x-buildingGridPosition.y)* 0.25f * tileWidth;
        Debug.Log(building.getSpriteCenter());
        */

        GameObject buildingInstance = GameObject.Instantiate(buildingGameObject);
        Building building = buildingInstance.GetComponent<Building>();
        building.grid = this;
        building.gridPosition = gridPosition;

        //buildingInstance.GetComponent<Building>().startEditing();
        //buildingInstance.transform.position = buildingPosition;
        changePositionOfBuilding(buildingInstance);
        if (isOnBoard(building) && !isOnBuilding(building))
        {
            building.lastGridPosition = building.gridPosition;
            buildings.Add(building);
        }
        buildingPlaced.Invoke(buildingInstance.GetComponent<Building>());
        return buildingInstance;
    }
    public void changePositionOfBuilding(GameObject buildingGameObject)
    {

        Building building = buildingGameObject.GetComponent<Building>();
        //if(isOnBoard(building)){
        foreach (IsometricTile tile in building.usingTiles)
        {
            tile.gridPicture.GetComponent<SpriteRenderer>().color = Color.black;
            tile.building = null;

        }
        building.usingTiles.Clear();
        //Debug.Log("Grid: width="+building.width+" height="+building.height);
        if (isOnBoard(building))
        {
            for (int x = (int)building.gridPosition.x; x < building.gridPosition.x + building.width; x++)
            {
                for (int y = (int)building.gridPosition.y; y < building.gridPosition.y + building.height; y++)
                {
                    if (grid[x, y].building == null)
                    {
                        grid[x, y].building = buildingGameObject;
                        building.usingTiles.Add(grid[x, y]);
                        grid[x, y].gridPicture.GetComponent<SpriteRenderer>().color = Color.blue;
                    }
                }
            }
        }
        //if(canPlaceOn(building.gridPosition.x,building.gridPosition.y))
        Vector3 buildingGridPosition = building.gridPosition + building.offset;
        Vector3 buildingPosition = new Vector3();
        buildingPosition.x = (buildingGridPosition.x + buildingGridPosition.y) * 0.5f * tileWidth;
        buildingPosition.y = (buildingGridPosition.x - buildingGridPosition.y) * 0.25f * tileWidth;
        building.transform.position = buildingPosition;
        //int sortingOrder = (int)(Mathf.Pow(size,2)-Mathf.Round(Mathf.Sqrt(Mathf.Pow(0f-buildingGridPosition.x,2f)+Mathf.Pow(size-buildingGridPosition.y,2f))));
        int sortingOrder = (int)(width * width - buildingGridPosition.x);
        building.buildingRenderer.sortingOrder = sortingOrder;
        if (!isOnBoard(building) || isOnBuilding(building))
        {
            building.buildingRenderer.color = Color.red;
        }
        else
        {
            building.buildingRenderer.color = Color.white;
        }
        //}
    }
    public Vector3 getClosestGridPosition(Vector3 position)
    {


        float newY = 0.5f * (position.x / (0.5f * tileWidth) - position.y / (0.25f * tileWidth));
        float newX = position.x / (0.5f * tileWidth) - newY;
        return new Vector3(Mathf.Round(newX), Mathf.Round(newY), 0);
    }
    public void removeBuilding(Building building)
    {

        foreach (IsometricTile tile in building.usingTiles)
        {
            tile.gridPicture.GetComponent<SpriteRenderer>().color = Color.black;
            tile.building = null;

        }
        buildings.Remove(building);
    }

    public string Serialize()
    {
        // Dictionary<string, dynamic> dict = new Dictionary<string, object>();
        // dict["width"] = width;
        // dict["length"] = length;
        // List<dynamic> serializedBuildings = new List<dynamic>();
        // foreach (Building building in buildings)
        // {
        //     Dictionary<string, dynamic> buildingDict = new Dictionary<string, dynamic>();
        //     buildingDict["x"] = building.gridPosition.x;
        //     buildingDict["y"] = building.gridPosition.y;
        //     buildingDict["width"] = building.width;
        //     buildingDict["height"] = building.height;
        //     buildingDict["type"] = "Building";
        //     serializedBuildings.Add(buildingDict);
        // }
        // dict["buildings"] = serializedBuildings;
        BaseData bd = new BaseData(this, buildings);
        return JsonConvert.SerializeObject(bd);
    }
    public static IsometricGrid Deserialize(string jsonString)
    {
        JObject json = JObject.Parse(jsonString);
        GameObject gridSprite = (GameObject)Resources.Load("Prefabs/Grid");
        float width = gridSprite.GetComponent<Renderer>().bounds.size.x;
        float widthWithEdgesMerged = width * 0.95f;
        IsometricGrid newGrid = new IsometricGrid(json["width"].Value<int>(), json["length"].Value<int>(), widthWithEdgesMerged);
        newGrid.buildingsJson = json["buildings"];


        Debug.Log(newGrid);
        return newGrid;
    }
    /*
    public void placeBuilding(GameObject buildingGameObject){
        
        GameObject clone = GameObject.Instantiate(buildingGameObject);
        
        Building building = clone.GetComponent<Building>();

        building.getGridRenderPosition();
        building.renderPosition.x = (building.gridRenderPosition.x+building.gridRenderPosition.y)* 0.5f * tileWidth;
        building.renderPosition.y = (building.gridRenderPosition.x-building.gridRenderPosition.y)* 0.25f * tileWidth;
        clone.transform.position = building.renderPosition;
        Debug.Log("Grid says : "+building.gridRenderPosition.x+" , "+building.gridRenderPosition.y);
        
        for (int x = (int)building.gridPosition.x; x <(int)building.gridPosition.x+ building.width; x++)
        {
            grid[x,(int)building.gridPosition.y].building = building;
            grid[x,(int)building.gridPosition.y].gridPicture.GetComponent<SpriteRenderer>().color = Color.blue;
        }
        for (int y = (int)building.gridPosition.y; y <(int)building.gridPosition.y+ building.width; y++)
            {
                grid[(int)building.gridPosition.x,y].building = building;
                grid[(int)building.gridPosition.x,y].gridPicture.GetComponent<SpriteRenderer>().color = Color.blue;
            }
    }*/
}

