using UnityEngine;

public class EnemySkeletonAnimationTriggers : MonoBehaviour
{
   private EnemySkeleton enemySkeleton;
   public DamageScaleData basicAttackScale;
   private Entity_Stats _stats;
   [SerializeField] private LayerMask whatIsPlayer;
   private void Awake()
   {
      enemySkeleton = GetComponentInParent<EnemySkeleton>();
      _stats = GetComponentInParent<Entity_Stats>();
   }
   public void AnimationTriggers()
   {
      enemySkeleton.AnimationTrigger();
   }

   public void AttackTrigger()
   {
      var colliders = Physics2D.OverlapCircleAll(enemySkeleton.attackCheck.position, enemySkeleton.attackCheckRadius, whatIsPlayer);

      foreach (var target in colliders)
      {
         var damageable = target.GetComponent<IDamageable>();
         var cloneDamageable = target.GetComponent<ICloneDamageable>();
         var attackData = _stats.GetAttackData(basicAttackScale);

         var physicalDamage = attackData.physicalDamage;
         var elementalDamage = attackData.elementalDamage;
         var element = attackData.element;

         var lastDamageTaken = physicalDamage + elementalDamage;
         
         damageable?.TakeDamage(physicalDamage, elementalDamage, element,target.transform, _stats.transform, attackData.isCritical);
         cloneDamageable?.CloneTakeDamage(lastDamageTaken);
         
         var statusHandler = target.GetComponent<Entity_StatusHandler>();
         if (element != ElementType.None)
            statusHandler?.ApplyStatusEffect(element, attackData.effectData);
         
      }
   }

   private void OpenCounterWindow() => enemySkeleton.OpenCounterAttackWindow();

   private void CloseCounterWindow() => enemySkeleton.CloseCounterAttackWindow();
}
