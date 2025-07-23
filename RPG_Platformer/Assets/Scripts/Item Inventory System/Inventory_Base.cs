using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory_Base : MonoBehaviour
{
    public static Inventory_Base Instance;
    public int maxInventorySize = 10;
    public List<Inventory_Item> itemList = new List<Inventory_Item>();
    public event Action OnInventoryChange;
    public bool CanAddItem() => itemList.Count < maxInventorySize;
    protected virtual void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }


    public void AddItem(Inventory_Item itemToAdd)
    {
        Inventory_Item itemInInventory = FindItem(itemToAdd.itemData);
        
        if(itemInInventory!= null && itemInInventory.CanAddStack())
            itemInInventory.AddStack();
        else
            itemList.Add(itemToAdd);
            
        OnInventoryChange?.Invoke();
    }

    protected void RemoveItem(Inventory_Item itemToRemove)
    {
        itemList.Remove(FindItem(itemToRemove.itemData));
        OnInventoryChange?.Invoke();
    }

    protected Inventory_Item FindItem(ItemDataSO itemData)
    {
       return itemList.Find(item => item.itemData == itemData);
    }
}
