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
        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.attackCheck.position, player.attackCheckRadius);
        foreach (var hit in colliders)
        {
            // if (hit.GetComponent<Enemy>() != null)
            //     hit.GetComponent<Enemy>().TakeDamage(damage);

            // if(hit.GetComponent<Chest>() != null)
            //     hit.GetComponent<Chest>().TakeDamage(damage);
            
            IDamageable damageable = hit.GetComponent<IDamageable>();
            damageable?.TakeDamage(_stats.GetPhysicalDamage(), hit.transform, transform);
        }
    }

    private void ThrowSword()
    {
        SkillManager.instance.swordSkill.CreateSword();
    }
    
}
