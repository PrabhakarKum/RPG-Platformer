using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UI_TreeNode : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    private UI_Manager _uiManager;
    private RectTransform _rectTransform;
    private UI_SkillTree _skillTree;
    private UI_TreeConnectHandler _connectHandler;

    [Header("Unlock Details")] 
    public UI_TreeNode[] neededNodes;  // required skills that must be unlocked before this one.
    public UI_TreeNode[] conflictNodes; // conflicting skills. If these are unlocked, you cannot unlock this one.
    public bool isUnlocked;
    public bool isLocked;
    
    
    [Header("Skill Details")]
    public Skill_DataSO skillData;
    [SerializeField] private string skillName;
    [SerializeField] private Image skillIcon;
    [SerializeField] private int skillCost;
    [SerializeField] private string lockedColorHex = "#373636";
    private Color _lastColor;

    private void Awake()
    {
        _skillTree = GetComponentInParent<UI_SkillTree>();
        _uiManager = GetComponentInParent<UI_Manager>();
        _rectTransform = GetComponent<RectTransform>();
        _connectHandler = GetComponent<UI_TreeConnectHandler>();
        UpdateIconColor(GetColorByHex(lockedColorHex));
    }

    private void OnEnable()
    {
        StartCoroutine(DelayedUnlock());
    }

    private IEnumerator DelayedUnlock()
    {
        yield return null;
        if (skillData.unlockedByDefault)
            UnlockSkill();
    }

    public void Refund()
    {
        if(isUnlocked == false || skillData.unlockedByDefault)
            return;
        
        isUnlocked = false;
        isLocked = false;
        UpdateIconColor(GetColorByHex(lockedColorHex));
        _connectHandler.UnlockConnectionImage(false);
        
        // skill manager and reset skill
    }
    private bool CanBeUnlocked()
    {
        if (isLocked || isUnlocked)
            return false;

        if (_skillTree.EnoughSkillPoints(skillCost) == false)
            return false;
        
        
        foreach (var node in neededNodes)
        {
            if (node.isUnlocked == false)
                return false;
        }
        
        foreach (var node in conflictNodes)
        {
            if (node.isUnlocked)
                return false;
        }

        return true;
    }

    private void LockConflictNodes()
    {
        foreach (var node in conflictNodes)
        {
            node.isLocked = true;
            node.LockChildNodes();
        }
    }

    private void LockChildNodes()
    {
        foreach (var node in _connectHandler.GetChildNodes())
        {
            node.isLocked = true;
            node.LockChildNodes();
        }
    }

    private void UnlockSkill()
    {
        isUnlocked = true;
        UpdateIconColor(Color.white);
        LockConflictNodes();
        
        _skillTree.RemoveSkillPoints(skillCost);
        _connectHandler.UnlockConnectionImage(true);

        _skillTree.skillManager.GetSkillByType(skillData.skillType).SetSkillUpgrade(skillData.upgradeData);
    }

    private void UpdateIconColor(Color color)
    {
        if(skillIcon == null)
            return;

        _lastColor = skillIcon.color;
        skillIcon.color = color;
    }
    
    public void OnPointerDown(PointerEventData eventData)
    {
        if (CanBeUnlocked())
            UnlockSkill();
        else if(isLocked)
            _uiManager.skillTooltip.LockedSkillEffect();
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        _uiManager.skillTooltip.ShowToolTip(true, _rectTransform, this);
        
        if (isUnlocked || isLocked)
            return;
        
        ToggleNodeHighlight(true);
        
        
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _uiManager.skillTooltip.ShowToolTip(false, _rectTransform);
        
        if (isUnlocked || isLocked)
            return;
        
        ToggleNodeHighlight(false);
    }

    private void ToggleNodeHighlight(bool highlight)
    {
        var highlightColor = Color.white * 0.9f;
        highlightColor.a = 1f;

        Color colorToApply = highlight ?  highlightColor: _lastColor;
        UpdateIconColor(colorToApply);
    }
    private Color GetColorByHex(string hexNumber)
    {
        ColorUtility.TryParseHtmlString(hexNumber, out var color);
        return color;
    }

    private void OnValidate()
    {
        if(skillData == null)
            return;

        skillName = skillData.displayName;
        skillIcon.sprite = skillData.icon;
        skillCost = skillData.cost;
        gameObject.name = "UI_TreeNode - " + skillName;
    }

    private void OnDisable()
    {
        if(isLocked || isUnlocked == false)
            UpdateIconColor(GetColorByHex(lockedColorHex));
        
        if(isUnlocked)
            UpdateIconColor(Color.white);
    }
}
