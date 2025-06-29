using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCounterAttackState : PlayerState
{
    public PlayerCounterAttackState(Player player, PlayerStateMachine playerStateMachine, string animBoolName) : base(player, playerStateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        stateTimer = player.counterAttackDuration;
        player.animator.SetBool("SuccessfullCounterAttack", false);
    }

    public override void Update()
    {
        base.Update();
        player.SetZeroVelocity();
        
        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.attackCheck.position, player.attackCheckRadius);
        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                if (hit.GetComponent<Enemy>().CanBeStunned())
                {
                    stateTimer = 10;
                    player.animator.SetBool("SuccessfullCounterAttack", true);
                }
            }
                
        }

        if (stateTimer < 0 || triggerCalled)
        {
            playerStateMachine.ChangeState(player.idleState);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
