using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillObject_CloneAnimator : MonoBehaviour
{
    private SkillObject_Clone clone;

    private void Awake()
    {
        clone = GetComponentInParent<SkillObject_Clone>();
    }

    private void AttackTrigger()
    {
        clone.PerformAttack();
    }

    private void TryTerminate(int currentAttackIndex)
    {
        if (currentAttackIndex == clone.maxAttacks)
            clone.HandleDeath();
    }
}
