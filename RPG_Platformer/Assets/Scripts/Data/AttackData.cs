using System;


[Serializable]
public class AttackData
{
    public float physicalDamage;
    public float elementalDamage;
    public bool isCritical;
    public ElementType element;
    public ElementalEffectData effectData;

    public AttackData(Entity_Stats entityStats, DamageScaleData scaleData)
    {
        physicalDamage = entityStats.GetPhysicalDamage(out isCritical, scaleData.physical);
        elementalDamage = entityStats.GetElementalDamage( out element ,scaleData.elemental);
        effectData = new ElementalEffectData(entityStats, scaleData);
    }
}
