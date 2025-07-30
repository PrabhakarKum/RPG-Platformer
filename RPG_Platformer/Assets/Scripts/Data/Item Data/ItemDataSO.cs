using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "Rpg Setup/ Item Data/ Material Item", fileName = "Material Data - ")]
public class ItemDataSO : ScriptableObject
{
    [Header("Merchant Details")] 
    [Range(0, 10000)]
    public int itemPrice = 100;
    public int minStackSizeAtShop = 1;
    public int maxStackSizeAtShop = 1;

    [Header("Drop Details")] 
    [Range(0, 1000)]
    public int itemRarity = 100;
    [Range(0, 100)]
    public float dropChance;
    [Range(0, 100)]
    public float maxDropChance = 65;
    
    [Header("Craft Details")]
    public Inventory_Item[] craftRecipe;
    
    [Header("Item Details")]
    public string itemName;
    public Sprite itemIcon;
    public ItemType itemType;
    public int maxStackSize = 1;

    [Header("Item Effect")]
    public ItemEffect_DataSO itemEffect;

    public void OnValidate()
    {
        dropChance = GetDropChance();
    }

    public float GetDropChance()
    {
        float maxRarity = 1000;
        float chance = (maxRarity - itemRarity + 1) / maxRarity * 100;

        return Mathf.Min(chance, maxDropChance);
    }

}
