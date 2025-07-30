using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Rpg Setup/ Item Data/ Item Effect /Heal Effect", fileName = "Item Effect Data Heal - ")]
public class ItemEffect_Heal : ItemEffect_DataSO
{
    [SerializeField] private float healPercent = 0.1f;

    public override void ExecuteEffect()
    {
        base.ExecuteEffect();
        Player player = PlayerManager.Instance.player;
        float healAmount = player.entityStats.GetMaxHealth() * healPercent;
        
        player.IncreaseHealth(healAmount);
    }
}
