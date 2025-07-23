using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashSkill : Skill_Base
{
    protected override void UseSkill()
    {
        if (Unlocked(SkillUpgradeType.Dash))
        {
            base.UseSkill();
        }
       
    }
    
    public void OnStartEffect(Transform clonePosition, Vector3 offset)
    {
        if (Unlocked(SkillUpgradeType.Dash_CloneOnStart) || Unlocked(SkillUpgradeType.Dash_CloneOnStartAndArrival))
        {
            CreateClone(clonePosition, offset);
        }

        if (Unlocked(SkillUpgradeType.Dash_ShardOnStart) || Unlocked(SkillUpgradeType.Dash_ShardOnStartAndArrival))
        {
            CreateShard();
        }
    }
    
    public void OnEndEffect(Transform clonePosition, Vector3 offset)
    {

        if (Unlocked(SkillUpgradeType.Dash_CloneOnStartAndArrival))
        {
            CreateClone(clonePosition, offset);
        }
            

        if (Unlocked(SkillUpgradeType.Dash_ShardOnStartAndArrival)) 
        {
            CreateShard();
        }
    }

    private void CreateShard()
    {
        skillManager.crystalSkill.CreateRawCrystal();
    }
    
    private void CreateClone(Transform clonePosition, Vector3 offset)
    {
        skillManager.cloneSkill.CreateClone(clonePosition, offset);
    }
    
    
}
