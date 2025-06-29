using Unity.VisualScripting;
using UnityEngine;

public class PlayerWallSlideState: PlayerState
{
    public PlayerWallSlideState(Player player, PlayerStateMachine playerStateMachine, string animBoolName) : base(player, playerStateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }
    public override void Exit()
    {
        base.Exit();
    }
    public override void Update()
    {
        base.Update();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            
            playerStateMachine.ChangeState(player.wallJumpState);
            return;
        }
        
        if (xInput != 0 && player.facingDirection != xInput)
        {
            playerStateMachine.ChangeState(player.idleState);
        }

        if (yInput < 0)
        {
            player.rigidBody.velocity = new Vector2(0, player.rigidBody.velocity.y);
        }
        else
        {
            player.rigidBody.velocity = new Vector2(0f, player.rigidBody.velocity.y * 0.7f);
        }
        
        if (player.IsGroundDetected())
        {
            playerStateMachine.ChangeState(player.idleState);
        }

        if (!player.IsWallDetected())
        {
            playerStateMachine.ChangeState(player.idleState);
        }
        
       
        
    }

}
