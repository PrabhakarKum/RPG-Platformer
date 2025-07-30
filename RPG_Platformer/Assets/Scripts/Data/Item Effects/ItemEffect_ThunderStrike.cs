using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;

[CreateAssetMenu(menuName = "Rpg Setup/ Item Data/ Item Effect /Thunder Strike", fileName = "Item Effect Data - Thunder Strike")]
public class ItemEffect_ThunderStrike : ItemEffect_DataSO
{
    [Header("Thunder Strike Details")]
    [SerializeField] private ElementalEffectData effectData;
    [SerializeField] private LayerMask whatIsEnemy;
    [SerializeField] private float percentThunderStrikeOnAttack = 0.15f;
    [SerializeField] private float lightningDamage;
    [SerializeField] private GameObject thunderStrikeVFX;
    
    public override void Subscribe(Player player)
    {
        base.Subscribe(player);
        player.playerCombat.OnTargetHit += TryThunderStrike;
    }
    
    public override void Unsubscribe()
    {
        base.Unsubscribe();
        player.playerCombat.OnTargetHit -= TryThunderStrike;
        
        player = null;
    }
    private void TryThunderStrike()
    {
        bool rollSucceeded = Random.Range(0f, 1f) < percentThunderStrikeOnAttack;
        if (!rollSucceeded)
            return; 
        
        var hitEnemies = Physics2D.OverlapCircleAll(player.transform.position, 0.5f, whatIsEnemy);
        foreach (Collider2D target in hitEnemies)
        {
            IDamageable damageable = target.GetComponent<IDamageable>();
            if (damageable == null) continue;
            
            bool targetGotHit = damageable.TakeDamage(0, lightningDamage, ElementType.Lightning, target.transform, player.transform, false);
            Entity_StatusHandler statusHandler = target.GetComponent<Entity_StatusHandler>();
            statusHandler?.ApplyStatusEffect(ElementType.Lightning, effectData);
            
            if(targetGotHit)
                player.playerVFX.CreateEffectOf(thunderStrikeVFX, target.transform);
            
        }
    }
}
