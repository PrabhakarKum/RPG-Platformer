using System;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "Rpg Setup/ Skill Data", fileName = "Skill Data - ")]
public class Skill_DataSO : ScriptableObject
{
    [Header("Skill Description")]
    public Sprite icon;
    public string displayName;
    [TextArea]
    public string description;
    
    [Header("Upgrades & Unlocks")]
    public int cost;
    public bool unlockedByDefault;
    public SkillType skillType;
    public UpgradeData upgradeData;
}

[Serializable]
public class UpgradeData
{
    public SkillUpgradeType upgradeType;
    public float cooldown;
    [FormerlySerializedAs("damageScale")] public DamageScaleData damageScaleData;
}
