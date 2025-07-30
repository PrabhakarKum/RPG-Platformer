using System;
using System.Text;

[Serializable]
public class Inventory_Item
{
    public ItemDataSO itemData;
    public int stackSize = 1;
    private string itemID;
    public ItemModifier[] modifiers { get; private set; }
    public ItemEffect_DataSO itemEffect;
    
    public int buyPrice { get; private set; }
    public int sellPrice { get; private set; }
    public Inventory_Item(ItemDataSO itemData)
    {
        this.itemData = itemData;
        itemEffect = itemData.itemEffect;
        buyPrice = itemData.itemPrice;
        sellPrice = (int)(itemData.itemPrice * 0.35f);
        

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
    
    public void AddItemEffect(Player player) => itemEffect?.Subscribe(player);
    public void RemoveItemEffect() =>  itemEffect?.Unsubscribe();

    private Equipment_DataSO EquipmentData()
    {
        if(itemData is Equipment_DataSO equipment)
            return equipment;
        
        return null;
    }

    public bool CanAddStack() => stackSize < itemData.maxStackSize;
    public void AddStack() => stackSize++;
    public void RemoveStack() => stackSize--;
    
    public string GetItemInfo()
    {
        StringBuilder sb = new StringBuilder();

        if (itemData.itemType == ItemType.Material)
        {
            sb.AppendLine("");
            sb.AppendLine("Used for Crafting");
            sb.AppendLine("");
            sb.AppendLine("");
            return sb.ToString();
        }

        if (itemData.itemType == ItemType.Consumable)
        {
            sb.AppendLine("");
            sb.AppendLine(itemEffect.effectDescription);
            sb.AppendLine("");
            sb.AppendLine("");
            return sb.ToString();
        }
        
        sb.AppendLine("");

        foreach (var mod in modifiers)
        {
            string modType = GetStatNameByType(mod.statType); 
            string modValue = IsPercentageStat(mod.statType) ? mod.value.ToString() + "%" : mod.value.ToString();
            sb.AppendLine("+ "+ modValue + " " + modType);
            
        }
        
        if(itemEffect != null)
        {
            sb.AppendLine("");
            sb.AppendLine("unique effect:");
            sb.AppendLine(itemEffect.effectDescription);
        }
        return sb.ToString();
    }
    
    private string GetStatNameByType(StatType statType)
    {
        switch (statType)
        {
            case StatType.MaxHealth: return "Max Health";
            case StatType.HealthRegeneration: return "HealthRegeneration";
            case StatType.Strength: return "Strength";
            case StatType.Agility: return "Agility";
            case StatType.Intelligence: return "Intelligence";
            case StatType.Vitality: return "Vitality";
            case StatType.AttackSpeed: return "Attack Speed";
            case StatType.Damage: return "Damage";
            case StatType.CriticalPower: return "Critical Power";
            case StatType.CriticalChance: return "Critical Chance";
            case StatType.ArmourReduction: return "Armour Reduction";
            case StatType.FireDamage: return "Fire Damage";
            case StatType.IceDamage: return "Ice Damage";
            case StatType.LightningDamage: return "Lightning Damage";
            case StatType.Armour: return "Armour";
            case StatType.Evasion: return "Evasion";
            case StatType.FireResistance: return "Fire Resistant";
            case StatType.IceResistance: return "Ice Resistant";
            case StatType.LightningResistance: return "Lightning Resistant";
            default:
                return "Unknown Stat";
        }
    }

    private bool IsPercentageStat(StatType statType)
    {
        switch (statType)
        {
            case StatType.CriticalChance:
            case StatType.CriticalPower:
            case StatType.ArmourReduction:
            case StatType.FireResistance:
            case StatType.IceResistance:
            case StatType.LightningResistance:
            case StatType.AttackSpeed:
            case StatType.Evasion:
                return true;
            default:
                return false;
        }
    }
}
