using UnityEngine;

public class PlayerPrimaryAttackState : PlayerState
{
    private int comboCounter;
    private float lastTimeAttacked;
    private float comboWindow = 2f;
    public PlayerPrimaryAttackState(Player player, PlayerStateMachine playerStateMachine, string animBoolName) : base(player, playerStateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        //xInput = 0; //need this to fix atk direction 
        if (comboCounter > 2 || Time.time >= lastTimeAttacked + comboWindow)
        {
            comboCounter = 0;
        }
        
        player.animator.SetInteger("ComboCounter", comboCounter);
        
        float attackDir = player.facingDirection;
        if (xInput != 0)
        {
            attackDir = xInput;
        }
        
        player.SetVelocity(player.attackMovement[comboCounter].x * attackDir, player.attackMovement[comboCounter].y);
        stateTimer = 0.1f;
    }
    public override void Exit()
    {
        base.Exit();

        player.StartCoroutine("BusyFor", 0.15f);
        comboCounter++;
        lastTimeAttacked = Time.time;
    }
    public override void Update()
    {
        base.Update();

        if (stateTimer < 0)
        {
           player.SetZeroVelocity();
        }
        if (triggerCalled)
        {
            playerStateMachine.ChangeState(player.idleState);
        }
    }

}
