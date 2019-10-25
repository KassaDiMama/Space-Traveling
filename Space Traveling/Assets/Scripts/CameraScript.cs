using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class CameraScript : MonoBehaviour
{
    // Start is called before the first frame update
    private Vector3 startPoint;
    private bool mouseDown = false;
    [HideInInspector]
    public bool canMove = true;
    public EventSystem eventSystem;
    void Start()
    {
        GameObject gridSprite = (GameObject)Resources.Load("Prefabs/Grid");
        float width = gridSprite.GetComponent<Renderer>().bounds.size.x;
        float widthWithEdgesMerged = width * 0.95f;
        IsometricGrid test = new IsometricGrid(20, 10, widthWithEdgesMerged);
        //test.placeGrid();
        //test.placeBuilding((GameObject)Resources.Load("Prefabs/Ground3x2"));
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            mouseDown = false;
        }
        if (canMove && !eventSystem.IsPointerOverGameObject())
        {
            if (Application.platform == RuntimePlatform.WindowsEditor)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    startPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    mouseDown = true;


                }
                if (mouseDown)
                {
                    Vector3 currentPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    Vector3 newPosition = startPoint - currentPoint;
                    Camera.main.transform.Translate(newPosition.x, newPosition.y, 0);
                }


            }
            else
            {
                if (Input.touchCount == 1)
                {
                    Touch touch = Input.GetTouch(0);

                    if (touch.phase == TouchPhase.Began)
                    {
                        startPoint = Camera.main.ScreenToWorldPoint(touch.position);
                    }
                    if (touch.phase == TouchPhase.Moved)
                    {
                        Vector3 currentPoint = Camera.main.ScreenToWorldPoint(touch.position);
                        Vector3 newPosition = startPoint - currentPoint;
                        Camera.main.transform.Translate(newPosition.x, newPosition.y, 0);
                    }
                }
            }
        }

    }
}
