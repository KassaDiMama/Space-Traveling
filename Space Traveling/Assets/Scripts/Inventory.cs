using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class InventoryItem
{
    public string prefabName;
    public int amount;
    public string type;

    public InventoryItem(string prefabName, int amount, string type)
    {
        this.prefabName = prefabName;
        this.amount = amount;
        this.type = type;
    }
}


public class Inventory
{
    public List<InventoryItem> inventoryList = new List<InventoryItem>();
    public Inventory()
    {

    }

    public bool itemPresent(string name)
    {
        foreach (InventoryItem item in inventoryList)
        {
            if (item.prefabName == name)
            {
                return true;
            }
        }
        return false;
    }
    public InventoryItem getItem(string name)
    {
        foreach (InventoryItem item in inventoryList)
        {
            if (item.prefabName == name)
            {
                return item;
            }
        }
        return null;
    }
    public void addItem(string name, string type, int amount = 1)
    {
        if (itemPresent(name))
        {
            getItem(name).amount += amount;
        }
        else
        {
            InventoryItem newItem = new InventoryItem(name, amount, type);
            inventoryList.Add(newItem);
        }
    }
    public void removeItem(string name, int amount = 1)
    {
        InventoryItem item = getItem(name);
        item.amount -= amount;
        if (item.amount <= 0)
        {
            inventoryList.Remove(item);
        }
    }
    public string Serialize()
    {
        return JsonConvert.SerializeObject(this);
    }
    public static Inventory Deserialize(string jsonString)
    {
        return JsonConvert.DeserializeObject<Inventory>(jsonString);
    }
}