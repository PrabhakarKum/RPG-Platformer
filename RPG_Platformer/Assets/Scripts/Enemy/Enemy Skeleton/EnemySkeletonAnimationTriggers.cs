using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySkeletonAnimationTriggers : MonoBehaviour
{
   public EnemySkeleton enemySkeleton;
   private Entity_Stats _stats;
   private void Awake()
   {
      _stats = GetComponentInParent<Entity_Stats>();
   }
   public void AnimationTriggers()
   {
      enemySkeleton.AnimationTrigger();
   }

   public void AttackTrigger()
   {
      Collider2D[] colliders = Physics2D.OverlapCircleAll(enemySkeleton.attackCheck.position, enemySkeleton.attackCheckRadius);

      foreach (var target in colliders)
      {
         var damageable = target.GetComponent<IDamageable>();
         var damage = _stats.GetPhysicalDamage(out var isCritical, 0.6f);
         var elementalDamage = _stats.GetElementalDamage(out var element, 0.6f);
         damageable?.TakeDamage(damage, elementalDamage, element,target.transform, _stats.transform, isCritical);
            
         if(element != ElementType.None)
            enemySkeleton.ApplyStatusEffect(target.transform, element);
      }
   }

   private void OpenCounterWindow() => enemySkeleton.OpenCounterAttackWindow();

   private void CloseCounterWindow() => enemySkeleton.CloseCounterAttackWindow();
}
