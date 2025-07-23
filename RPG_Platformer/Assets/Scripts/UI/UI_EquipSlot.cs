
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_EquipSlot : UI_ItemSlot
{
    public ItemType slotType;

    protected override void Start()
    {
        base.Start(); 
        OnValidate();
    }
    
    private void OnValidate()
    {
        gameObject.name = "UI_EquipSlot - " + slotType;
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        if(itemInSlot == null)
            return;
        
        inventory.UnEquipItem(itemInSlot);
    }
}
