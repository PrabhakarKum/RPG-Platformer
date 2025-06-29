using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAimSwordState : PlayerState
{
    // Start is called before the first frame update
    public PlayerAimSwordState(Player player, PlayerStateMachine playerStateMachine, string animBoolName) : base(player, playerStateMachine, animBoolName)
    {
    }


    public override void Enter()
    {
        base.Enter();
        player.skillManager.swordSkill.DotsActive(true);
    }

    public override void Update()
    {
        base.Update();
        
        player.SetZeroVelocity();
        
        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            playerStateMachine.ChangeState(player.idleState);
        }
        
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (player.transform.position.x > mousePos.x && player.facingDirection == 1)
            player.Flip();
        
        else if(player.transform.position.x < mousePos.x && player.facingDirection == -1)
            player.Flip();
    }

    public override void Exit()
    {
        base.Exit();
        player.StartCoroutine("BusyFor", 0.2f);
    }
}
