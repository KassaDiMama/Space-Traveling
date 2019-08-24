using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;


public class InventoryItemUI : MonoBehaviour
{
    // Start is called before the first frame update
    private bool itemDown=false;
    private bool itemDrag=false;
    private bool itemEntered=false;
    public string prefabName;
    [HideInInspector]
    public Transform contentTransform;
    [HideInInspector]
    public bool onGrid = false;
    public RectTransform inventoryTransform;
    [HideInInspector]
    public UnityEvent placedOnGrid = new UnityEvent();
    private bool moving;
    void Start()
    {
        EventTrigger trigger = GetComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerDown;
        entry.callback.AddListener((eventData)=>{onItemDown(eventData);});
        trigger.triggers.Add(entry);

        trigger = GetComponent<EventTrigger>();
        entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerUp;
        entry.callback.AddListener((eventData)=>{onItemUp(eventData);});
        trigger.triggers.Add(entry);

        trigger = GetComponent<EventTrigger>();
        entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.BeginDrag;
        entry.callback.AddListener((eventData)=>{onItemBeginDrag(eventData);});
        trigger.triggers.Add(entry);

        trigger = GetComponent<EventTrigger>();
        entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerExit;
        entry.callback.AddListener((eventData)=>{onItemPointerExit(eventData);});
        trigger.triggers.Add(entry);

        trigger = GetComponent<EventTrigger>();
        entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerEnter;
        entry.callback.AddListener((eventData)=>{onItemPointerEnter(eventData);});
        trigger.triggers.Add(entry);
    }

    // Update is called once per frame
    void Update()
    {
        if(itemDown && !itemEntered && !moving){
            moving = true;
            gameObject.transform.SetParent(GetComponentInParent<Canvas>().transform,false);
        }
        if(itemDown && moving){
            Vector2 pos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(GetComponentInParent<Canvas>().transform as RectTransform, Input.mousePosition, GetComponentInParent<Canvas>().worldCamera, out pos);
            transform.position = GetComponentInParent<Canvas>().transform.TransformPoint(pos);
            //Debug.Log(Input.mousePosition);
        
            RectTransformUtility.ScreenPointToLocalPointInRectangle(inventoryTransform as RectTransform, Input.mousePosition, GetComponentInParent<Canvas>().worldCamera, out pos);
            if(!inventoryTransform.rect.Contains(pos) && !onGrid){
                onGrid=true;
                placedOnGrid.Invoke();
                //Destroy(gameObject);
            }
        }
    }
    public void onItemDown(BaseEventData eventData){
        //gameObject.transform.SetParent(GetComponentInParent<Canvas>().transform,false);
        itemDown = true;

    }
    public void onItemUp(BaseEventData eventData){
        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(inventoryTransform as RectTransform, Input.mousePosition, GetComponentInParent<Canvas>().worldCamera, out pos);
        //transform.position = inventoryTransform.TransformPoint(pos);
        //Debug.Log(inventoryTransform.TransformPoint(Input.mousePosition));
        //Debug.Log(inventoryTransform.rect);
        if(inventoryTransform.rect.Contains(pos)){
            gameObject.transform.SetParent(contentTransform);
        }
        itemDown = false;
        moving=false;
    }

    public void onItemBeginDrag(BaseEventData eventData){
        itemDrag = true;
        
    }
    public void onItemEndDrag(BaseEventData eventData){
        itemDrag = false;
        
    }
    public void onItemPointerEnter(BaseEventData eventData){
        itemEntered = true;
        
    }
    public void onItemPointerExit(BaseEventData eventData){
        itemEntered = false;
        
    }
}
