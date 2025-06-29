using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBlackHoleState : PlayerState
{
    private float flyTime = 0.5f;
    private bool skillUsed;
    private float defaultGravityScale;
    
    public PlayerBlackHoleState(Player player, PlayerStateMachine playerStateMachine, string animBoolName) : base(player, playerStateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        defaultGravityScale = player.rigidBody.gravityScale;
        
        skillUsed = false;
        stateTimer = flyTime;
        player.rigidBody.gravityScale = 0;
    }

    public override void Update()
    {
        base.Update();
        if(stateTimer > 0)
            player.rigidBody.velocity = new Vector2(0f, 3f);

        if (stateTimer < 0)
        {
            player.rigidBody.velocity = new Vector2(0f, -0.1f);
            if (!skillUsed)
            {
                if(player.skillManager.blackHoleSkill.CanUseSkill())
                    skillUsed = true;
            }
        }
        
        if(player.skillManager.blackHoleSkill.BlackHoleFinished())
            playerStateMachine.ChangeState(player.airState);
        
        //WE ARE EXITING THE STATE FROM BLACK_HOLE_SKILL_CONTROLLER WHEN ALL THE ATTACKS ARE OVER//
    }

    public override void Exit()
    {
        base.Exit();
        player.rigidBody.gravityScale = defaultGravityScale;
        player.MakeTransparent(false);
    }
}
