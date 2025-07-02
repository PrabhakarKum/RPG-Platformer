using System;
using UnityEngine.Serialization;

[Serializable]
public class Stat_DefenceGroup
{
    //Physical Defense
    public Stat armour;
    public Stat evasion;
    
    // Elemental Resistant
    public Stat fireResistant;
    public Stat iceResistant;
    public Stat lightningResistant;
}
