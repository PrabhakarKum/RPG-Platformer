using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeathState : PlayerState
{
    public PlayerDeathState(Player player, PlayerStateMachine playerStateMachine, string animBoolName) : base(player, playerStateMachine, animBoolName)
    {
    }


    public override void Enter()
    {
        base.Enter();
        
    }

    public override void Update()
    {
        base.Update();
        xInput = 0;
        yInput = 0;
        player.rigidBody.simulated = false;
        //player.gameObject.layer = LayerMask.NameToLayer("IgnoreEnemy");
    }

    public override void Exit()
    {
        base.Exit();
    }
}
