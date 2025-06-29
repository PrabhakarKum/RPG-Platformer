using Unity.VisualScripting;
using UnityEngine;

public class PlayerDashState : PlayerState
{
    public PlayerDashState(Player player, PlayerStateMachine playerStateMachine, string animBoolName) : base(player, playerStateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.skillManager.cloneSkill.CreateClone(player.transform, Vector2.zero);
        stateTimer = player.dashDuration;
    }
    public override void Exit()
    {
        base.Exit();
        player.SetVelocity(0, player.rigidBody.velocity.y);
    }
    public override void Update()
    {
        base.Update();
        
        player.SetVelocity(player.dashSpeed * player.dashDirection, 0);
        
        if (stateTimer < 0f)
        {
            playerStateMachine.ChangeState(player.idleState);
        }

        if (!player.IsGroundDetected() && player.IsWallDetected())
        {
            playerStateMachine.ChangeState(player.wallSlideState);
        }
    }

}
