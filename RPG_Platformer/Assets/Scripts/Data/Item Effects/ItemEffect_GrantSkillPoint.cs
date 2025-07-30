using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Rpg Setup/ Item Data/ Item Effect /grant skill point", fileName = "Item Effect Data - Grant Skill Point ")]
public class ItemEffect_GrantSkillPoint : ItemEffect_DataSO
{
    [SerializeField] private int skillPointsToGrant = 1;
    public override void ExecuteEffect()
    {
        base.ExecuteEffect();
        UI_Manager uiManager = FindFirstObjectByType<UI_Manager>();
        uiManager.skillTreeUI.AddSkillPoints(skillPointsToGrant);
    }
}
