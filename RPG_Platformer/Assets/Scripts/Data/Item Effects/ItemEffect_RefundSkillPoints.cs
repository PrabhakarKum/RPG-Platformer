using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Rpg Setup/ Item Data/ Item Effect /Refund Skill Points", fileName = "Item Effect Data Heal - Refund All Skill Points")]
public class ItemEffect_RefundSkillPoints : ItemEffect_DataSO
{
    public override void ExecuteEffect()
    {
        base.ExecuteEffect();
        var uiManager = FindFirstObjectByType<UI_Manager>();
        uiManager.skillTreeUI.RefundAllSkills();
    }
}
