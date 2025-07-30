using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Rpg Setup/ Item Data/ Equipment Item", fileName = "Equipment Data - ")]
public class Equipment_DataSO : ItemDataSO
{
   [Header("Item Modifiers")]
   public ItemModifier[] modifiers;
   
}

[Serializable]
public class ItemModifier
{
    public StatType statType;
    public float value;
}
