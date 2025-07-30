using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Rpg Setup/ Item Data/ Item Effect /Heal On Doing Damage", fileName = "Item Effect Data Heal - Heal On physical damage ")]
public class ItemEffect_HealOnDoingDamage : ItemEffect_DataSO
{
    [SerializeField] private float percentHealedOnAttack = 0.2f;
    public override void ExecuteEffect()
    {
        base.ExecuteEffect();
        
    }

    public override void Subscribe(Player player)
    {
        base.Subscribe(player);
        player.playerCombat.OnDoingPhysicalDamage += HealedOnDoingDamage;
    }

    public override void Unsubscribe()
    {
        base.Unsubscribe();
        player.playerCombat.OnDoingPhysicalDamage -= HealedOnDoingDamage;
        player = null;
    }

    private void HealedOnDoingDamage(float damage)
    {
        player.IncreaseHealth(damage * percentHealedOnAttack);
    }
}
