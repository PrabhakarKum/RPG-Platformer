using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory_Player : Inventory_Base
{
    public int gold = 10000;
    public static Inventory_Player Instance;
    private Player player;
    public List<Inventory_EquipmentSlot> equipList;
    public Inventory_Storage storage { get; private set; }
    
    protected override void Awake()
    {
        base.Awake();
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
        
        player = GetComponent<Player>();
        storage = FindFirstObjectByType<Inventory_Storage>();
    }

    public void TryEquipItem(Inventory_Item item)
    {
        
        var inventoryItem = FindItem(item.itemData);
       
        var matchingSlots = equipList.FindAll(slot => slot.slotType == item.itemData.itemType);
        
        // trying to find empty slot and equip item
        foreach (var slot in matchingSlots)
        {
            if(slot.HasItem() == false)
            {
                EquipItem(inventoryItem, slot);
                return;
            }
        }
        
        // No empty slot found, replacing the first matching slot
        var slotToReplace = matchingSlots[0];
        var itemToUnequipped = slotToReplace.equippedItem;
        
        UnequipItem(itemToUnequipped, slotToReplace != null);
        EquipItem(inventoryItem,slotToReplace);
    }
    
    private void EquipItem(Inventory_Item itemToEquip, Inventory_EquipmentSlot slot)
    {
        var savedHealthPercent = player.GetHealthPercent();
        
        slot.equippedItem = itemToEquip;
        slot.equippedItem.AddModifiers(player.stats);
        slot.equippedItem.AddItemEffect(player);

        player.SetHealthToPercent(savedHealthPercent);
        RemoveOneItem(itemToEquip);
    }
    
    public void UnequipItem(Inventory_Item itemToUnEquip, bool replacingItem = false)
    {
        if (CanAddItem(itemToUnEquip) == false && replacingItem == false)
        {
            Debug.Log($"Can't unequipped item {itemToUnEquip.itemData.itemType}");
            return;
        }

        var savedHealthPercent = player.GetHealthPercent();
        
        var slotToUnEquip = equipList.Find(slot => slot.equippedItem == itemToUnEquip);
        if (slotToUnEquip != null)
            slotToUnEquip.equippedItem = null;
        
        itemToUnEquip.RemoveModifiers(player.stats);
        itemToUnEquip.RemoveItemEffect();
        
        player.SetHealthToPercent(savedHealthPercent);
        AddItem(itemToUnEquip);
    }
}
