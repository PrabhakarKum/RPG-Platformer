using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{
    public UI_SkillToolTip skillTooltip { get; private set; }
    public UI_ItemToolTip itemTooltip { get; private set; }
    public UI_StatToolTip statTooltip { get; private set; }
    public UI_SkillTree skillTreeUI { get; private set; }
    public UI_Inventory inventoryUI { get; private set; }
    public UI_Storage storageUI { get; private set; }
    public UI_Craft craftUI { get; private set; }
    
    public UI_Merchant merchantUI { get; private set; }

    private Inventory_Storage currentStorage;
    
    public GameObject uiCanvas;         // The main Canvas object
    public GameObject buttonsPanel;

    private bool uiActive = false;
    
    private void Awake()
    {
        skillTooltip = GetComponentInChildren<UI_SkillToolTip>();
        itemTooltip = GetComponentInChildren<UI_ItemToolTip>();
        statTooltip = GetComponentInChildren<UI_StatToolTip>();
        skillTreeUI = GetComponentInChildren<UI_SkillTree>(true);
        inventoryUI = GetComponentInChildren<UI_Inventory>(true);
        storageUI = GetComponentInChildren<UI_Storage>(true);
        craftUI = GetComponentInChildren<UI_Craft>(true);
        merchantUI = GetComponentInChildren<UI_Merchant>(true);

    }
    
    void Start()
    {
        uiCanvas.SetActive(false); // Hide UI when game starts
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            uiActive = !uiActive;
            uiCanvas.SetActive(uiActive);
            ToggleMainUI(uiActive);
        }
    }
    
    private void ToggleMainUI(bool isVisible)
    {
        if (isVisible)
        {
            ToggleInventoryUI();
            buttonsPanel.SetActive(true);
        }
        else
        {
            buttonsPanel.SetActive(false);
            SwitchOffAllTooltips();
        }
    }

    public void SwitchOffAllTooltips()
    {
        skillTooltip.ShowToolTip(false, null);
        itemTooltip.ShowToolTip(false, null);
        statTooltip.ShowToolTip(false, null);
    }
    public void ToggleSkillTreeUI()
    {
        skillTreeUI.gameObject.SetActive(true);
        
        inventoryUI.gameObject.SetActive(false);
        storageUI.gameObject.SetActive(false);

        skillTooltip.ShowToolTip(false, null);
    }
    
    public void ToggleInventoryUI()
    {
        inventoryUI.gameObject.SetActive(true);
        
        skillTreeUI.gameObject.SetActive(false);
        storageUI.gameObject.SetActive(false);
        
        itemTooltip.ShowToolTip(false, null);
        statTooltip.ShowToolTip(false, null);
    }

    public void ToggleStorageUI()
    {
        if (currentStorage == null) return;
        
        var playerInventory = PlayerManager.Instance.player.GetComponent<Inventory_Player>();

        if (playerInventory == null) return;
        currentStorage.SetInventory(playerInventory);
        storageUI.SetupStorage(currentStorage);
        
        storageUI.gameObject.SetActive(true);
            
        skillTreeUI.gameObject.SetActive(false);
        inventoryUI.gameObject.SetActive(false);

    }
    
    public void SetDefaultStorage(Inventory_Storage storage)
    {
        currentStorage = storage;
    }
}
