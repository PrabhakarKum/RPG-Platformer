using System;

[Serializable]
public class Stat_OffenceGroup
{
    public Stat attackSpeed;
    
    //Physical Damage
    public Stat damage;
    public Stat criticalPower;
    public Stat criticalChance;
    public Stat armourReduction;

    //Elemental Damage
    public Stat fireDamage;
    public Stat iceDamage;
    public Stat lightningDamage;
}
