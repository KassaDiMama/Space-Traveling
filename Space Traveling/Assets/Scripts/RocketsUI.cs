using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

using TMPro;



public class RocketsUI : MonoBehaviour
{
    // Start is called before the first frame update


    void Start()
    {
        //main.inventory.addItem("Ground3x2",1);
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void show()
    {
        GetComponent<CanvasGroup>().alpha = 1f;
    }
    public void hide()
    {
        GetComponent<CanvasGroup>().alpha = 0f;
    }

}
