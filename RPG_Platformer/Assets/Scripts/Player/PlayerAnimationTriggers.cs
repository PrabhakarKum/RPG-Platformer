using UnityEngine;

public class PlayerAnimationTriggers : MonoBehaviour
{
    private Player player;
    public DamageScaleData basicAttackScale;
    
    private Entity_Stats _stats;
    [SerializeField] private LayerMask whatIsEnemy;

    private void Awake()
    {
        player = GetComponentInParent<Player>();
        _stats = GetComponentInParent<Entity_Stats>();
    }

    private void AnimationTriggers()
    {
        player.AnimationTrigger();
    }

    private void AttackTriggers()
    {
        var colliders = Physics2D.OverlapCircleAll(player.attackCheck.position, player.attackCheckRadius, whatIsEnemy);
        foreach (var target in colliders)
        {
            var damageable = target.GetComponent<IDamageable>();
            var statusHandler = target.GetComponent<Entity_StatusHandler>();
            
            var attackData = _stats.GetAttackData(basicAttackScale);
            var physicalDamage = attackData.physicalDamage;
            var elementalDamage = attackData.elementalDamage;
            var element = attackData.element;
            
            damageable?.TakeDamage(physicalDamage, elementalDamage, element,target.transform, _stats.transform, attackData.isCritical);
            
            if (element != ElementType.None)
                statusHandler?.ApplyStatusEffect(element, attackData.effectData);
        }
    }

    private void ThrowSword()
    {
        SkillManager.Instance.swordSkill.CreateSword();
    }
    
}
