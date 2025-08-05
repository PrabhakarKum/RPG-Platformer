using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class UI_ItemToolTip : UI_Tooltip
{
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemType;
    [SerializeField] private TextMeshProUGUI itemDescription;
    [SerializeField] private TextMeshProUGUI itemPrice;
    [SerializeField] private Transform merchantInfo;
    [SerializeField] private Transform inventoryInfo;
    

    public void ShowToolTip(bool show, RectTransform targetRect, Inventory_Item itemToShow, bool buyPrice = false, bool showMerchantInfo = false)
    {
        base.ShowToolTip(show, targetRect);
        
        merchantInfo.gameObject.SetActive(showMerchantInfo);
        inventoryInfo.gameObject.SetActive(!showMerchantInfo);
        
        int price = buyPrice ? itemToShow.buyPrice : Mathf.FloorToInt(itemToShow.sellPrice);
        int totalPrice = price * itemToShow.stackSize;

        string fullStackPrice = ($"Price:{price} x {itemToShow.stackSize} = {totalPrice}g.");
        string singleStackPrice = ($"Price:{price}.");
        
        itemPrice.text = itemToShow.stackSize > 1 ? fullStackPrice : singleStackPrice;
        itemName.text = itemToShow.itemData.itemName;
        itemType.text = itemToShow.itemData.itemType.ToString();
        itemDescription.text = itemToShow.GetItemInfo();
    }

    
}
