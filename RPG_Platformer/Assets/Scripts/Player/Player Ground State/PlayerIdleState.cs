using UnityEngine;

public class PlayerIdleState : PlayerGroundState
{
    public PlayerIdleState(Player player, PlayerStateMachine playerStateMachine, string animBoolName) : base( player, playerStateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.ResetJumps();
        player.SetZeroVelocity();
    }


    public override void Exit()
    {
        base.Exit();
    }
    public override void Update()
    {
        base.Update();
        
        if(xInput == player.facingDirection && player.IsWallDetected())
            return;
        
        if (xInput != 0 && !player.isBusy)
        {
            playerStateMachine.ChangeState(player.moveState);
        }
    }
}
