using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class Building : MonoBehaviour
{
    // Start is called before the first frame update
    public Vector3 gridPosition;
    public Vector3 offset;
    public float width;
    public float height;
    [HideInInspector]
    public Vector3 center;
    [HideInInspector]
    public IsometricGrid grid;
    public bool mouseDown = false;
    public SpriteRenderer buildingRenderer;
    [HideInInspector]
    public List<IsometricTile> usingTiles = new List<IsometricTile>();
    public bool editing = false;
    public UnityEvent isEditing;
    public UnityEvent completeEditing;
    public UnityEvent stopEditing;
    private Vector3 lastMousePosition;
    public UnityEvent onMouseDown;
    public UnityEvent removeBuilding;
    public UnityEvent hasRotated;
    public Transform outlines;
    public Vector3 lastGridPosition = new Vector3(-1f, -1f, -1f);
    public bool rotated;
    public string type;



    void Start()
    {
        getSpriteCenter();
        transform.Find("EditButtons/Vinkje").gameObject.GetComponent<GameObjectButton>().onMouseDown.AddListener(OnCompleteEditing);
        transform.Find("EditButtons/Kruisje").gameObject.GetComponent<GameObjectButton>().onMouseDown.AddListener(OnStopEditing);
        transform.Find("EditButtons/InInventory").gameObject.GetComponent<GameObjectButton>().onMouseDown.AddListener(OnInInventory);
        transform.Find("EditButtons/Rotation").gameObject.GetComponent<GameObjectButton>().onMouseDown.AddListener(OnRotation);
    }
    void OnMouseEnter()
    {

    }
    void OnMouseDown()
    {
        onMouseDown.Invoke();
        mouseDown = true;

        if (editing)
        {
            Camera.main.GetComponent<CameraScript>().canMove = false;
        }
        else
        {
            Invoke("checkIfEdit", 0.8f);
            lastMousePosition = Input.mousePosition;
        }

    }
    void OnMouseUp()
    {
        Camera.main.GetComponent<CameraScript>().canMove = true;
        mouseDown = false;
    }
    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            OnMouseUp();
        }
        if (editing && mouseDown)
        {
            gridPosition = grid.getClosestGridPosition(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            grid.changePositionOfBuilding(gameObject);
        }
    }
    public Vector3 getSpriteCenter()
    {
        SpriteRenderer SR = gameObject.GetComponent<SpriteRenderer>();
        center = new Vector3(width / 2, height / 2, 0);
        return center;
    }
    /*
    int tileCounter = 0;
    for (int x = 0; x < width; x++)
    {
        for (int y = 0; y < height; y++)
        {
            tileCounter+=1;
            averageOffset+=new Vector2(x,y);
        }
    }
    averageOffset/=tileCounter;
    gridRenderPosition = gridPosition+averageOffset;
    Debug.Log("Building says : "+gridRenderPosition.x+" , "+gridRenderPosition.y);
    */
    private void checkIfEdit()
    {
        if (mouseDown && lastMousePosition == Input.mousePosition && !editing)
        {
            startEditing();

        }
    }
    public void startEditing()
    {
        showEditButtons();
        isEditing.Invoke();

    }
    public void OnCompleteEditing()
    {
        editing = false;
        completeEditing.Invoke();
        hideEditButtons();
    }
    public void OnStopEditing()
    {
        editing = false;
        stopEditing.Invoke();
        hideEditButtons();

    }
    public void OnInInventory()
    {
        removeBuilding.Invoke();
        //Debug.Log("wtf");
    }
    public void OnRotation()
    {
        //Debug.Log("Before: widht-"+width+" height-"+height);
        width += height;
        height = width - height;
        width -= height;
        offset.x += offset.y;
        offset.y = offset.x - offset.y;
        offset.x -= offset.y;
        //Debug.Log("After: widht-"+width+" height-"+height);
        /*
        GetComponent<SpriteRenderer>().flipX = !GetComponent<SpriteRenderer>().flipX;
        if(GetComponent<SpriteRenderer>()!=buildingRenderer){
            buildingRenderer.flipX = !buildingRenderer.flipX;
        }
        outlines.gameObject.GetComponent<SpriteRenderer>().flipX = !outlines.gameObject.GetComponent<SpriteRenderer>().flipX;
        */
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        Transform editButtons = transform.Find("EditButtons");
        editButtons.localScale = new Vector3(-editButtons.localScale.x, editButtons.localScale.y, editButtons.localScale.z);
        rotated = !rotated;
        hasRotated.Invoke();
    }
    public void showEditButtons()
    {
        int editButtonDir = (int)(Mathf.Abs(transform.localScale.x) / transform.localScale.x);
        transform.Find("EditButtons").localScale = new Vector3(editButtonDir * 0.1f, 0.1f, 0.1f);
    }
    public void hideEditButtons()
    {
        transform.Find("EditButtons").localScale = new Vector3(0f, 0f, 0f);
    }

    public void startSelected()
    {
        outlines.localScale = new Vector3(1f, 1f, 1f);
        if (transform.Find("SelectedMenu"))
        {
            SelectedMenu selectedMenu = transform.Find("SelectedMenu").GetComponent<SelectedMenu>();
            if (selectedMenu)
            {
                int menuDir = (int)(Mathf.Abs(transform.localScale.x) / transform.localScale.x);
                selectedMenu.show(menuDir);
            }
        }

    }
    public void stopSelected()
    {
        outlines.localScale = new Vector3(0f, 0f, 0f);
        if (transform.Find("SelectedMenu"))
        {
            SelectedMenu selectedMenu = transform.Find("SelectedMenu").GetComponent<SelectedMenu>();
            if (selectedMenu)
            {
                selectedMenu.hide();
            }
        }
    }

}
