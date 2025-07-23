using System;

[Serializable]
public class Inventory_Item
{
    public ItemDataSO itemData;
    public int stackSize = 1;
    private string itemID;
    public ItemModifier[] modifiers { get; private set; }
    public Inventory_Item(ItemDataSO itemData)
    {
        this.itemData = itemData;
        var equipment = EquipmentData();
        modifiers = equipment != null ? equipment.modifiers : Array.Empty<ItemModifier>();

        itemID = itemData.itemName + " - " + Guid.NewGuid();
    }
    
    public void AddModifiers(Entity_Stats playerStats)
    {
        foreach (var modifier in modifiers)
        { 
            Stat statToModify = playerStats.GetStatByType(modifier.statType);
            statToModify.AddModifier(modifier.value, itemID);
        }
    }
    
    public void RemoveModifiers(Entity_Stats playerStats)
    {
        foreach (var modifier in modifiers)
        { 
            Stat statToModify = playerStats.GetStatByType(modifier.statType);
            statToModify.RemoveModifier(itemID);
        }
    }

    private Equipment_DataSO EquipmentData()
    {
        if(itemData is Equipment_DataSO equipment)
            return equipment;
        
        return null;
    }

    public bool CanAddStack() => stackSize < itemData.maxStackSize;

    public void AddStack() => stackSize++;
    
    public void RemoveStack() => stackSize--;
}
