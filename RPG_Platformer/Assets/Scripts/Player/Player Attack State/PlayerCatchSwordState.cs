using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCatchSwordState : PlayerState
{
    private Transform sword;
    
    public PlayerCatchSwordState(Player player, PlayerStateMachine playerStateMachine, string animBoolName) : base(player, playerStateMachine, animBoolName)
    {
        
    }

    public override void Enter()
    {
        base.Enter();
        sword = player.sword.transform;
        if (player.transform.position.x > sword.position.x && player.facingDirection == 1)
            player.Flip();
        
        else if(player.transform.position.x < sword.position.x && player.facingDirection == -1)
            player.Flip();
        
        player.rigidBody.velocity = new Vector2(player.swordReturnImpact * -player.facingDirection, player.rigidBody.velocity.y);
    }

    public override void Update()
    {
        base.Update();
        if(triggerCalled)
            player.playerStateMachine.ChangeState(player.idleState);
    }

    public override void Exit()
    {
        base.Exit();
        player.StartCoroutine("BusyFor", 0.1f);
    }
}
