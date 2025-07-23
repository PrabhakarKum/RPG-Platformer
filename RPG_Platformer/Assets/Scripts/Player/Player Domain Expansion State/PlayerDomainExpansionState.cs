using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDomainExpansionState : PlayerState
{
   private Vector2 originalPosition;
   private float originalGravity;
   private float maxDistanceToGoUp;
   private bool isFloating;
   private bool createdDomain;
   
   public PlayerDomainExpansionState(Player player, PlayerStateMachine playerStateMachine, string animBoolName) : base(player, playerStateMachine, animBoolName)
   {
      
   }

   public override void Enter()
   {
      base.Enter();
      
      originalPosition = player.transform.position;
      originalGravity = player.rigidBody.gravityScale;
      maxDistanceToGoUp = GetAvailableRiseDistance();
      player.SetCanTakeDamage(false);
      
      player.SetVelocity(0, player.riseSpeed);
   }

   public override void Update()
   {
      base.Update();
      
      if (Vector2.Distance(originalPosition, player.transform.position) >= maxDistanceToGoUp && isFloating == false)
         Floating();
         
      if (isFloating)
      {
         SkillManager.Instance.domainExpansion.DoSpellCasting();

         if (stateTimer < 0)
         {
            isFloating = false;
            player.rigidBody.gravityScale = originalGravity;
            playerStateMachine.ChangeState(player.idleState);
         }
         
      }
   }

   public override void Exit()
   {
      base.Exit();
      createdDomain = false;
      player.SetCanTakeDamage(true);
   }

   private void Floating()
   {
      isFloating = true;
      stateTimer = SkillManager.Instance.domainExpansion.GetDomainDuration();
      player.rigidBody.velocity = Vector2.zero;
      player.rigidBody.gravityScale = 0;

      if (createdDomain == false)
      {
         createdDomain = true;
         SkillManager.Instance.domainExpansion.CreateDomain();
      }
   }
   private float GetAvailableRiseDistance()
   {
      RaycastHit2D hit = Physics2D.Raycast(player.transform.position, Vector2.up, player.riseMaxDistance, player.whatIsGround);

      return (float)(hit.collider != null ? hit.distance - 0.5 : player.riseMaxDistance);
   }
}
