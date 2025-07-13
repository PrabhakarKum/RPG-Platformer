using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAimSwordState : PlayerState
{
    private Camera camera;

    // Start is called before the first frame update
    public PlayerAimSwordState(Player player, PlayerStateMachine playerStateMachine, string animBoolName) : base(player, playerStateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        camera = Camera.main;
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
        
        DirectionToMouse();
    }

    public override void Exit()
    {
        base.Exit();
        player.StartCoroutine("BusyFor", 0.2f);
    }
    
    private void DirectionToMouse()
    {
        if (camera == null) return;
        Vector2 worldMousePosition = camera.ScreenToWorldPoint(Input.mousePosition);
        Vector2 playerPosition = player.transform.position;

        if (playerPosition.x > worldMousePosition.x && player.facingDirection == 1)
            player.Flip();
        
        else if(playerPosition.x < worldMousePosition.x && player.facingDirection == -1)
            player.Flip();
    }
}
