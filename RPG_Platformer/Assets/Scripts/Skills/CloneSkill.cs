using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneSkill : Skill
{
    [Header("Clone Info")]
    [SerializeField] private GameObject clonePrefab;
    [SerializeField] private float cloneDuration;
    [Space]
    [SerializeField] private bool canAttack;

    public void CreateClone(Transform clonePosition, Vector3 offset)
    {
       GameObject clone = Instantiate(clonePrefab);
       clone.GetComponent<CloneSkillController>().SetupClone(clonePosition, cloneDuration, canAttack, offset);
    }
    
}
