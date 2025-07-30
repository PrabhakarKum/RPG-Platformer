using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_CraftListButton : MonoBehaviour
{
    [SerializeField] private ItemListDataSO craftData;
    private UI_CraftSlot[] craftSlots;
    
    
    public void SetCraftSlots(UI_CraftSlot[] craftSlots) => this.craftSlots = craftSlots;

    public void UpdateCraftSlot()
    {
        if (craftData == null)
        {
            Debug.Log("You need to assign craft list data");
            return;
        }

        foreach (var slot in craftSlots)
        {
            slot.gameObject.SetActive(false);
        }

        Debug.Log($"Craft Slots: {craftSlots.Length}");
        Debug.Log($"Craft Length: {craftData.itemList.Length}");
        
        for (var i = 0; i < craftData.itemList.Length ; i++)
        {
            var itemData = craftData.itemList[i];
            craftSlots[i].gameObject.SetActive(true);
            craftSlots[i].SetupButton(itemData);
            
        }
    }
}
