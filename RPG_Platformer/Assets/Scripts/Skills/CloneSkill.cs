using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneSkill : Skill_Base
{
    [Header("Clone Info")]
    [SerializeField] private GameObject clonePrefab;
    public float cloneDuration;

    [Header("Attack Upgrades")] 
    [SerializeField] private int maxAttacks = 3;
    [SerializeField] private float duplicateChance = 0.3f;

    [Header("Heal Wisp Upgrades")] 
    [SerializeField] private float damagePercentHealed = 0.3f;
    [SerializeField] private float cooldownReducedInSeconds;
    protected override void UseSkill()
    {
        //use skill
        base.UseSkill();
    }

    protected override void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            CreateClone(player.transform, Vector2.zero);
        }
    }
    
    public void CreateClone(Transform clonePosition, Vector3 offset)
    {
        var clone = Instantiate(clonePrefab);
        clone.GetComponent<SkillObject_Clone>().SetupClone(this, clonePosition, offset);
    }

    public int GetMaxAttacks()
    {
        switch (upgradeType)
        {
            case SkillUpgradeType.CloneSkill_SingleAttack or SkillUpgradeType.CloneSkill_ChanceToDuplicate:
                return 1;
            case SkillUpgradeType.CloneSkill_MultiAttack:
                return maxAttacks;
            default:
                return 0;
        }
    }

    public float GetDuplicateChance()
    {
        if (upgradeType != SkillUpgradeType.CloneSkill_ChanceToDuplicate)
            return 0;

        return duplicateChance;
    }

    public bool ShouldBeWisp()
    {
        return upgradeType is SkillUpgradeType.CloneSkill_HealWish 
            or SkillUpgradeType.CloneSkill_CleanseWisp 
            or SkillUpgradeType.CloneSkill_CooldownWisp;
    }

    public float GetPercentOfDamageHealed()
    {
        if (ShouldBeWisp() == false)
            return 0;

        return damagePercentHealed;
    }

    public float GetCoolReduceInSeconds()
    {
        if (upgradeType != SkillUpgradeType.CloneSkill_CooldownWisp)
            return 0;

        return cooldownReducedInSeconds;
    }

    public bool CanRemoveNegativeEffects()
    {
        return upgradeType == SkillUpgradeType.CloneSkill_CleanseWisp;
    }
}
