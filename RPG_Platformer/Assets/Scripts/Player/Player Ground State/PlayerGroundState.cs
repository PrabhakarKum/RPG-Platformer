using UnityEngine;

public class PlayerGroundState : PlayerState
{
    public PlayerGroundState(Player player, PlayerStateMachine playerStateMachine, string animBoolName) : base(player, playerStateMachine, animBoolName)
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

        if (Input.GetKeyDown(KeyCode.R))
        {
            playerStateMachine.ChangeState(player.blackHoleState);
        }

        if (Input.GetKeyDown(KeyCode.Mouse1) && HasNoSword())
        {
            playerStateMachine.ChangeState(player.aimSwordState);
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            playerStateMachine.ChangeState(player.counterAttackState);
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            playerStateMachine.ChangeState(player.primaryAttackState);
        }
        
        if (Input.GetKeyDown(KeyCode.Space) && player.IsGroundDetected())
        {
            playerStateMachine.ChangeState(player.jumpState);
        }
        
    }

    private bool HasNoSword()
    {
        if (!player.sword)
        {
            return true;
        }
        
        player.sword.GetComponent<SwordSkillController>().ReturnSword();
        return false;
    }
}
