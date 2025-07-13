using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SkillUpgradeType 
{
    None,
    
    // -------- Dash Tree ------------
    Dash, // dash to avoid damage
    Dash_CloneOnStart, // Creates a clone when dash Starts
    Dash_CloneOnStartAndArrival, // Creates a clone when dash Starts and ends
    Dash_ShardOnStart, // Creates a shard when dash starts
    Dash_ShardOnStartAndArrival, // Creates a shard when dash starts and ends
    
    // -------- Crystal Tree ----------- 
    
    Crystal, // The crystal explodes when touched by an enemy or time goes up
    Crystal_MoveToEnemy,  // Crystal will move towards the nearest enemy
    Crystal_MultiCast, // Crystal ability can have upto N charges. You can cast them all in a row. 
    Crystal_Teleport, // You can swap places with the last crystal you created 
    Crystal_TeleportHpRewind, // When you swap places with crystal, your HP % is same ass it was when you created crystal
    
    
    // -------- Sword Tree ----------
    SwordThrow, // you can throw the sword to damage enemies from range
    SwordThrow_Spin, // your sword will spin at one point and damage enemies
    SwordThrow_Pierce, // Pierce sword will pierce N targets
    SwordThrow_Bounce, // Bounce sword will bounce between enemies
    
}
