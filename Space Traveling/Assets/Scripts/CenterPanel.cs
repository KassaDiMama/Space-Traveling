using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CenterPanel : MonoBehaviour
{
    public Button inventoryButton;
    public Button friendsButton;
    public RectTransform inventoryPanel;
    public Main main;
    public bool inventoryUp=false;
    // Start is called before the first frame update
    void Start()
    {
        inventoryButton.onClick.AddListener(OnInventoryButtonClicked);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnInventoryButtonClicked(){
        if(main.editing){
            moveInventoryDown();
        }else{
            moveInventoryUp();
        }
    }
    public void moveInventoryDown(){
        if(main.currentlyEditing==null){
            main.hideGrid();
        }
        inventoryUp = false;
        GetComponent<RectTransform>().DOAnchorPos(new Vector2(0,-56),0.25f);
    }
    public void moveInventoryUp(){
        main.showGrid();
        inventoryUp = true;
        GetComponent<RectTransform>().DOAnchorPos(new Vector2(0,56),0.25f);
    }
}
