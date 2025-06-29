using UnityEngine;

public class PlayerMoveState : PlayerGroundState
{
    public PlayerMoveState(Player player, PlayerStateMachine playerStateMachine, string animBoolName) : base(player, playerStateMachine, animBoolName)
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
        
        player.SetVelocity(xInput * player.moveSpeed , player.rigidBody.velocity.y);
        
        if (xInput == 0 || player.IsWallDetected())
        {
            playerStateMachine.ChangeState(player.idleState);
        }
    }
}
