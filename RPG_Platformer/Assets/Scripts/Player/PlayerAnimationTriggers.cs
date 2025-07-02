using System;
using UnityEngine;

public class PlayerAnimationTriggers : MonoBehaviour
{
    public Player player;
    private Entity_Stats _stats;

    private void Awake()
    {
        _stats = GetComponentInParent<Entity_Stats>();
    }

    private void AnimationTriggers()
    {
        player.AnimationTrigger();
    }

    private void AttackTriggers()
    {
        var colliders = Physics2D.OverlapCircleAll(player.attackCheck.position, player.attackCheckRadius);
        foreach (var target in colliders)
        {
            var damageable = target.GetComponent<IDamageable>();
            var damage = _stats.GetPhysicalDamage(out var isCritical);
            var elementalDamage = _stats.GetElementalDamage(out var element, 0.6f);
            damageable?.TakeDamage(damage, elementalDamage, element,target.transform, _stats.transform, isCritical);
            
            if(element != ElementType.None)
                player.ApplyStatusEffect(target.transform, element);
        }
    }

    private void ThrowSword()
    {
        SkillManager.instance.swordSkill.CreateSword();
    }
    
}
