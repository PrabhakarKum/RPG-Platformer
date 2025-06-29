using UnityEngine;

public class PlayerWallJumpState : PlayerState
{
    public PlayerWallJumpState(Player player, PlayerStateMachine playerStateMachine, string animBoolName) : base(player, playerStateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        stateTimer = 0.4f;
        player.currentJumpCount++;
        player.SetVelocity(2 * -player.facingDirection, player.jumpForce);
    }
    public override void Exit()
    {
        base.Exit();
    }
    public override void Update()
    {
        base.Update();

        if (stateTimer < 0)
        {
            playerStateMachine.ChangeState(player.airState);
        }

        if (player.IsGroundDetected())
        {
            playerStateMachine.ChangeState(player.idleState);
        }
    }
}
