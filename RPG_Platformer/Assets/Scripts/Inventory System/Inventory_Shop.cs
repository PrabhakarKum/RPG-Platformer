using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory_Shop : Inventory_Base
{
    private Inventory_Player inventoryPlayer;

    [SerializeField] private ItemListDataSO shopData;
    [SerializeField] private int minItemsAmount = 4;
    [SerializeField] private int maxItemsAmount = 10;

    public void SetInventory(Inventory_Player inventoryPlayer) => this.inventoryPlayer = inventoryPlayer;
    protected override void Awake()
    {
        base.Awake();
        
        FillShopList();
    }

    public void TryBuyItem(Inventory_Item itemToBuy, bool buyFullstack)
    {
        int amountToBuy = buyFullstack ? itemToBuy.stackSize : 1;

        for (int i = 0; i < amountToBuy; i++)
        {
            if (inventoryPlayer.gold < itemToBuy.buyPrice)
            {
                Debug.Log("Not enough money");
                return;
            }

            if (itemToBuy.itemData.itemType == ItemType.Material)
            {
                var materialToAdd = new Inventory_Item(itemToBuy.itemData);
                materialToAdd.stackSize = 1;
                inventoryPlayer.storage.AddMaterialToStash(materialToAdd);
                //inventoryPlayer.storage.AddMaterialToStash(itemToBuy);
            }
            else
            {
                if (inventoryPlayer.CanAddItem(itemToBuy))
                {
                    var itemToAdd = new Inventory_Item(itemToBuy.itemData);
                    inventoryPlayer.AddItem(itemToAdd);
                }
                else
                {
                    Debug.Log("Inventory is full");
                    break;
                }
                
            }
            
            inventoryPlayer.gold -= itemToBuy.buyPrice;
            RemoveOneItem(itemToBuy);
        }
        TriggerUpdateUI();
    }

    public void TrySellItem(Inventory_Item itemToSell, bool sellFullstack)
    {
        int amountToSell = sellFullstack ? itemToSell.stackSize : 1;

        for (int i = 0; i < amountToSell; i++)
        {
            int sellPrice = Mathf.FloorToInt(itemToSell.sellPrice);
            
            inventoryPlayer.gold += sellPrice;
            inventoryPlayer.RemoveOneItem(itemToSell);
        }
        TriggerUpdateUI();
    }
    
    public void FillShopList()
    {
        itemList.Clear();
        List<Inventory_Item> possibleItem = new List<Inventory_Item>();

        foreach (var itemData in shopData.itemList)
        {
            int randomisedStack = Random.Range(itemData.minStackSizeAtShop, itemData.maxStackSizeAtShop + 1);
            int finalStack = Mathf.Clamp(randomisedStack, 1, itemData.maxStackSize);

            Inventory_Item itemToAdd = new Inventory_Item(itemData);
            itemToAdd.stackSize = finalStack;
            
            possibleItem.Add(itemToAdd);
        }
        
        int randomItemAmount = Random.Range(minItemsAmount, maxItemsAmount + 1);
        int finalAmount = Mathf.Clamp(randomItemAmount, 1,possibleItem.Count);


        for (int i = 0; i < finalAmount; i++)
        {
            var randomIndex = Random.Range(0, possibleItem.Count);
            var item = possibleItem[randomIndex];

            if (CanAddItem(item))
            {
                possibleItem.Remove(item);
                AddItem(item);
            }
        }
        
        TriggerUpdateUI();
    }
    
    
}
