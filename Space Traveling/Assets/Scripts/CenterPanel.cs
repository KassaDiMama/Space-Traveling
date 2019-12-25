using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CenterPanel : MonoBehaviour
{
    public Button inventoryButton;
    public Button rocketsButton;
    public Button friendsButton;
    public InventoryUI inventoryPanel;
    public RocketsUI rocketsPanel;
    public FriendsUI friendsPanel;
    public Main main;
    public bool rocketsPanelUp = false;
    public bool inventoryPanelUp = false;
    public bool friendsPanelUp = false;
    // Start is called before the first frame update
    void Start()
    {
        inventoryButton.onClick.AddListener(OnInventoryButtonClicked);
        rocketsButton.onClick.AddListener(OnRocketsButtonClicked);
        friendsButton.onClick.AddListener(OnFriendsButtonClicked);
    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnFriendsButtonClicked()
    {
        inventoryPanel.refreshUI();
        if (friendsPanelUp)
        {
            friendsPanelUp = false;
            GetComponent<RectTransform>().DOAnchorPos(new Vector2(0, -56), 0.25f);

            inventoryPanel.hide();
            rocketsPanel.hide();
            friendsPanel.show();
        }
        else
        {
            GetComponent<RectTransform>().DOAnchorPos(new Vector2(0, 56), 0.25f);
            friendsPanelUp = true;
            inventoryPanel.hide();
            rocketsPanel.hide();
            friendsPanel.show();
        }
    }
    void OnInventoryButtonClicked()
    {
        inventoryPanel.refreshUI();
        if (main.editing && inventoryPanelUp)
        {
            moveInventoryDown();
            friendsPanel.hide();
            //moveInventoryDown();
        }
        else
        {
            moveinventoryPanelUp();
            friendsPanel.hide();
        }
    }
    void OnRocketsButtonClicked()
    {
        inventoryPanel.refreshUI();
        if (rocketsPanelUp)
        {
            moveRocketsDown();
            inventoryPanel.hide();
            friendsPanel.hide();
            rocketsPanel.show();
        }
        else
        {
            moveRocketsUp();
            inventoryPanel.hide();
            friendsPanel.hide();
            rocketsPanel.show();
        }
    }
    private void MoveCenterPanelDown()
    {
        GetComponent<RectTransform>().DOAnchorPos(new Vector2(0, -56), 0.25f);
        if (main.currentlyEditing == null)
        {
            main.hideGrid();
        }
        inventoryPanelUp = false;
        rocketsPanelUp = false;
    }
    private void MoveCenterPanelUp()
    {
        GetComponent<RectTransform>().DOAnchorPos(new Vector2(0, 56), 0.25f);
        inventoryPanelUp = true;
        rocketsPanelUp = true;
    }
    public void moveInventoryDown()
    {
        if (main.currentlyEditing == null)
        {
            main.hideGrid();
        }
        inventoryPanelUp = false;
        rocketsPanelUp = false;
        GetComponent<RectTransform>().DOAnchorPos(new Vector2(0, -56), 0.25f);
    }
    public void moveinventoryPanelUp()
    {
        main.showGrid();
        inventoryPanelUp = true;
        rocketsPanelUp = false;
        GetComponent<RectTransform>().DOAnchorPos(new Vector2(0, 56), 0.25f);
        inventoryPanel.show();
        rocketsPanel.hide();
    }

    public void moveRocketsDown()
    {

        rocketsPanelUp = false;
        GetComponent<RectTransform>().DOAnchorPos(new Vector2(0, -56), 0.25f);

    }
    public void moveRocketsUp()
    {
        main.hideGrid();
        rocketsPanelUp = true;
        inventoryPanelUp = false;
        GetComponent<RectTransform>().DOAnchorPos(new Vector2(0, 56), 0.25f);
        inventoryPanel.hide();
        rocketsPanel.show();
    }
}
