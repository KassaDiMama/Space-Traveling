using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameObjectButton : MonoBehaviour
{
    // Start is called before the first frame update
    public UnityEvent onMouseDown;
    public UnityEvent onMouseUp;
    public UnityEvent onMouseClick;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnMouseDown(){
        onMouseDown.Invoke();
    }
    public void OnMouseUp(){
        onMouseUp.Invoke();
    }
    public void OnMouseClick(){
        onMouseClick.Invoke();
    }
}
