using UnityEngine;

public class EnemySkeletonAnimationTriggers : MonoBehaviour
{
   public EnemySkeleton enemySkeleton;
   public DamageScaleData basicAttackScale;
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
      var colliders = Physics2D.OverlapCircleAll(enemySkeleton.attackCheck.position, enemySkeleton.attackCheckRadius);

      foreach (var target in colliders)
      {
         var damageable = target.GetComponent<IDamageable>();
         var attackData = _stats.GetAttackData(basicAttackScale);

         var physicalDamage = attackData.physicalDamage;
         var elementalDamage = attackData.elementalDamage;
         var element = attackData.element;
         
         damageable?.TakeDamage(physicalDamage, elementalDamage, element,target.transform, _stats.transform, attackData.isCritical);
         
         var statusHandler = target.GetComponent<Entity_StatusHandler>();
         if (element != ElementType.None)
            statusHandler?.ApplyStatusEffect(element, attackData.effectData);
         
      }
   }

   private void OpenCounterWindow() => enemySkeleton.OpenCounterAttackWindow();

   private void CloseCounterWindow() => enemySkeleton.CloseCounterAttackWindow();
}
