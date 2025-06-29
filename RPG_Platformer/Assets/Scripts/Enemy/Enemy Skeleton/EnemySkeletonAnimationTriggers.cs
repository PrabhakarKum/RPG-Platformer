using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySkeletonAnimationTriggers : MonoBehaviour
{
   public EnemySkeleton enemySkeleton;
   [SerializeField] private float damageAmount;
   public void AnimationTriggers()
   {
      enemySkeleton.AnimationTrigger();
   }

   public void AttackTrigger()
   {
      Collider2D[] colliders = Physics2D.OverlapCircleAll(enemySkeleton.attackCheck.position, enemySkeleton.attackCheckRadius);

      foreach (var hit in colliders)
      {
         if (hit.GetComponent<Player>() != null) 
             hit.GetComponent<Player>().TakeDamage(damageAmount, hit.transform, transform);
      }
   }

   private void OpenCounterWindow() => enemySkeleton.OpenCounterAttackWindow();

   private void CloseCounterWindow() => enemySkeleton.CloseCounterAttackWindow();
}
