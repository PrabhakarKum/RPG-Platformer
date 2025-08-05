using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory_Base : MonoBehaviour
{
    public int maxInventorySize = 10;
    public List<Inventory_Item> itemList = new List<Inventory_Item>();
    public event Action OnInventoryChange;

    public bool CanAddItem(Inventory_Item itemToAdd)
    {
        bool hasStackable = FindStackable(itemToAdd) != null;
        return hasStackable || itemList.Count < maxInventorySize;
    }
    public Inventory_Item FindStackable(Inventory_Item itemToAdd)
    {
        List<Inventory_Item> stackableItems = itemList.FindAll(item => item.itemData == itemToAdd.itemData);

        foreach (var stackableItem in stackableItems)
        {
            if (stackableItem.CanAddStack())
                return stackableItem;
        }
        return null;
    }
    
    protected virtual void Awake()
    {
       
    }

    public void TryUseItem(Inventory_Item itemToUse)
    {
        Inventory_Item consumable = itemList.Find(item => item == itemToUse);
        
        if(consumable == null)
            return;
        
        consumable.itemEffect.ExecuteEffect();

        if (consumable.stackSize > 1)
            consumable.RemoveStack();
        else
            RemoveOneItem(consumable);
        
        OnInventoryChange?.Invoke();
    }

    public void AddItem(Inventory_Item itemToAdd)
    {
        Inventory_Item itemInInventory = FindStackable(itemToAdd);
        
        if (itemInInventory != null)
            itemInInventory.AddStack();
        else
            itemList.Add(itemToAdd);
        
        OnInventoryChange?.Invoke();
    }

    public void RemoveOneItem(Inventory_Item itemToRemove)
    {
        Inventory_Item itemInInventory = itemList.Find(item => item == itemToRemove);
        
        if(itemInInventory.stackSize > 1)
            itemInInventory.RemoveStack();
        else
            itemList.Remove(itemToRemove);
        
        OnInventoryChange?.Invoke();
    }

    public void RemoveFullStack(Inventory_Item itemToRemove)
    {
        if (itemToRemove == null)
            return;
        
        for (int i = 0; i < itemToRemove.stackSize; i++)
        {
            RemoveOneItem(itemToRemove);
        }
    }

    protected Inventory_Item FindItem(ItemDataSO itemData)
    {
       return itemList.Find(item => item.itemData == itemData);
    }
    
    public void TriggerUpdateUI() => OnInventoryChange?.Invoke();
}
