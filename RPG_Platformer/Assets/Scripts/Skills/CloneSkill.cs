using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneSkill : Skill_Base
{
    [Header("Clone Info")]
    [SerializeField] private GameObject clonePrefab;
    [SerializeField] private float cloneDuration;
    [Space]
    [SerializeField] private bool canAttack;

    
    protected override void UseSkill()
    {
        //use skill
        base.UseSkill();
    }
    
    public void CreateClone(Transform clonePosition, Vector3 offset)
    {
        var clone = Instantiate(clonePrefab);
        clone.GetComponent<CloneSkillController>().SetupClone(clonePosition, cloneDuration, canAttack, offset);
    }
    
}
