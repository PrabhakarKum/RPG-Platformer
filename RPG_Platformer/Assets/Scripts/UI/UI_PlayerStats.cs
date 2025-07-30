using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_PlayerStats : MonoBehaviour
{
    private UI_StatSlot[] uiStatSlots;
    private Inventory_Player inventoryPlayer;

    private void Awake()
    {
        uiStatSlots = GetComponentsInChildren<UI_StatSlot>();
    }

    private void Start()
    {
        inventoryPlayer = Inventory_Player.Instance;
        if (inventoryPlayer != null) inventoryPlayer.OnInventoryChange += UpdateStatsUI;
        UpdateStatsUI();
    }

    private void UpdateStatsUI()
    {
        foreach (var statSlot in uiStatSlots)
            statSlot.UpdateStatValue();
    }
}
