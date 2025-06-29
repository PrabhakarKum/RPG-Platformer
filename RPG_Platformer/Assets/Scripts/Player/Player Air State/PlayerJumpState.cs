using UnityEngine;

public class PlayerJumpState : PlayerState
{
    public PlayerJumpState(Player player, PlayerStateMachine playerStateMachine, string animBoolName) : base(player, playerStateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.currentJumpCount++;
        player.rigidBody.velocity = new Vector2(player.rigidBody.velocity.x, player.jumpForce);
    }

    public override void Update()
    {
        base.Update();
        
        if (player.rigidBody.velocity.y < 0)
        {
            playerStateMachine.ChangeState(player.airState);
        }
    }

    public override void Exit()
    {
        base.Exit();
        
        
    }
}
