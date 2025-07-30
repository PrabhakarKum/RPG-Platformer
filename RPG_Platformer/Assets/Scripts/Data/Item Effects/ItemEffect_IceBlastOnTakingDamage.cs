using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Rpg Setup/ Item Data/ Item Effect /Ice Blast", fileName = "Item Effect Data - Ice Blast On Taking Damage ")]
public class ItemEffect_IceBlastOnTakingDamage : ItemEffect_DataSO
{
    [SerializeField] private ElementalEffectData effectData;
    [SerializeField] private float iceDamage;
    [SerializeField] private LayerMask whatIsEnemy;
    [Space]
    [SerializeField] private float healthPercentTrigger = 0.25f;
    [SerializeField] private float cooldown = 40f; 
    private float lastTimeUsed = -99f;

    [Header("VFX objects")] 
    [SerializeField] private GameObject iceBlastVFX;
    [SerializeField] private GameObject hitVFX;
    
    public override void ExecuteEffect()
    {
        base.ExecuteEffect();
        
        var noCooldown = Time.time >= lastTimeUsed + cooldown;
        var reachedThreshold = player.GetHealthPercent() <= healthPercentTrigger;
        if (noCooldown && reachedThreshold)
        {
            player.playerVFX.CreateEffectOf(iceBlastVFX, player.transform);
            lastTimeUsed = Time.time;
            DamageEnemiesWithIce();
        }
    }

    private void DamageEnemiesWithIce()
    {
        var hitEnemies = Physics2D.OverlapCircleAll(player.transform.position, 2f, whatIsEnemy);
        foreach (Collider2D target in hitEnemies)
        {
            IDamageable damageable = target.GetComponent<IDamageable>();
            if (damageable == null) continue;
            
            bool targetGotHit = damageable.TakeDamage(0, iceDamage, ElementType.Ice, target.transform, player.transform, false);
            Entity_StatusHandler statusHandler = target.GetComponent<Entity_StatusHandler>();
            statusHandler?.ApplyStatusEffect(ElementType.Ice, effectData);
            
            if(targetGotHit)
                player.playerVFX.CreateEffectOf(hitVFX, player.transform);
            
        }
    }
    public override void Subscribe(Player player)
    {
        base.Subscribe(player);
        player.OnTakingDamage += ExecuteEffect;
        lastTimeUsed = -cooldown; 
    }

    public override void Unsubscribe()
    {
        base.Unsubscribe();
        player.OnTakingDamage -= ExecuteEffect;
        player = null;
    }
}
