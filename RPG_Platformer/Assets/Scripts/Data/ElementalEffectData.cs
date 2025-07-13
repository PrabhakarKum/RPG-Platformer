using System;
public class ElementalEffectData
{
    public float chillDuration;
    public float chillSlowMultiplier;
    
    public float burnDuration;
    public float burnDamage;
    
    public float shockDuration;
    public float shockDamage;
    public float shockCharge;


    public ElementalEffectData(Entity_Stats entityStats, DamageScaleData damageScale)
    {
        chillDuration = damageScale.chillDuration;
        chillSlowMultiplier = damageScale.chillSlowMultiplier;

        burnDuration = damageScale.burnDuration;
        burnDamage = entityStats.offence.fireDamage.GetValue() * damageScale.burnDamageScale;

        shockDuration = damageScale.shockDuration;
        shockDamage = entityStats.offence.lightningDamage.GetValue() * damageScale.shockDamageScale;
        shockCharge = damageScale.shockCharge;
    }
}
