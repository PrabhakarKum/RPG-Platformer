using UnityEngine;

public class PlayerAirState : PlayerState
{
    public PlayerAirState(Player player, PlayerStateMachine playerStateMachine, string animBoolName) : base(player, playerStateMachine, animBoolName)
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

        if (player.IsWallDetected())
        {
            playerStateMachine.ChangeState(player.wallSlideState);
        }
        
        if (player.IsGroundDetected())
        {
            playerStateMachine.ChangeState(player.idleState);
        }

        if (xInput != 0)
        {
            player.SetVelocity(player.moveSpeed * 0.8f * xInput, player.rigidBody.velocity.y);
        }
        
        if (Input.GetKeyDown(KeyCode.Space) && player.CanJump())
        {
            playerStateMachine.ChangeState(player.jumpState);
        }
    }
}
