using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Assertions.Comparers;

public class Player : Entity, IDamageable
{
    [Header("Attack Details")]
    public bool hasSword = true;
    public Vector2[] attackMovement;
    public float counterAttackDuration = 0.2f; 
    public bool isBusy {get; private set;}

    
    public static event Action OnPlayerDeath;
    
    
    #region Information
    [Header("Movement Info")]
    public float moveSpeed = 10f;
    public float swordReturnImpact = 1.5f;
    
    [Header("Dash Info")]
    public float dashSpeed = 10f;
    public float dashDuration = 0.5f;
    public float dashDirection {get; private set;}
    
    [Header("Jump Info")]
    public float jumpForce = 10f;
    public int maxJumps = 2;
    public int currentJumpCount = 0;
    public bool CanJump() => currentJumpCount < maxJumps;
    
    #endregion
    
   #region Components
   public SkillManager skillManager {get; private set;}
   public UI_Manager uiManager {get; private set;}
   public Entity_StatusHandler statusHandler {get; private set;}
   public GameObject sword{get; private set;}
   
   #endregion
    
    #region States
    public PlayerStateMachine playerStateMachine {get; private set;}
    public PlayerIdleState idleState {get; private set;}
    public PlayerMoveState moveState {get; private set;}
    public PlayerJumpState jumpState {get; private set;}
    public PlayerAirState airState {get; private set;}
    public PlayerDashState dashState {get; private set;}
    public PlayerWallSlideState wallSlideState {get; private set;}
    public PlayerWallJumpState wallJumpState {get; private set;}
    public PlayerPrimaryAttackState primaryAttackState {get; private set;}
    public PlayerCounterAttackState counterAttackState{get; private set;}
    public PlayerAimSwordState aimSwordState {get; private set;}
    public PlayerCatchSwordState catchSwordState {get; private set;}
    public PlayerBlackHoleState blackHoleState{get; private set;}
    public PlayerDeathState deathState {get; private set;}
    
    #endregion

    protected override void Awake()
    {
        base.Awake();

        uiManager = FindAnyObjectByType<UI_Manager>();
        statusHandler = GetComponent<Entity_StatusHandler>();
        playerStateMachine = new PlayerStateMachine();
        
        idleState = new PlayerIdleState(this, playerStateMachine, "Idle");
        moveState = new PlayerMoveState(this, playerStateMachine, "Move");
        jumpState = new PlayerJumpState(this, playerStateMachine, "Jump");
        airState  = new PlayerAirState(this, playerStateMachine, "Jump");
        dashState = new PlayerDashState(this, playerStateMachine, "Dash");
        deathState = new PlayerDeathState(this, playerStateMachine, "Death");
        
        wallSlideState = new PlayerWallSlideState(this, playerStateMachine, "WallSlide");
        wallJumpState =  new PlayerWallJumpState(this, playerStateMachine, "Jump");
        
        primaryAttackState =  new PlayerPrimaryAttackState(this, playerStateMachine, "Attack");
        counterAttackState =  new PlayerCounterAttackState(this, playerStateMachine, "CounterAttack");
        aimSwordState      = new PlayerAimSwordState(this, playerStateMachine, "AimSword");
        catchSwordState    = new PlayerCatchSwordState(this, playerStateMachine, "CatchSword");
        
        blackHoleState = new PlayerBlackHoleState(this, playerStateMachine, "Jump");
    }

    protected override void Start()
    {
        base.Start();
        skillManager = SkillManager.Instance;
        playerStateMachine.Initialize(idleState);
    }

    protected override void Update()
    {
        base.Update();
        playerStateMachine.currentState.Update();
        CheckForDashInput();
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OpenSkillTree();
        }
    }

    protected override IEnumerator SlowDownEntityCoroutine(float duration, float slowMultiplier)
    {
        Debug.Log($"player slow multiplier: {slowMultiplier}");
        var originalMoveSpeed = moveSpeed;
        var originalJumpForce = jumpForce;
        var originalAnimSpeed = animator.speed;
        var originalDashSpeed = dashSpeed;
        var originalAttackMovement = attackMovement;
        var speedMultiplier = 1 - slowMultiplier;
        
        Debug.Log("player speed multiplier: "+ speedMultiplier);
        moveSpeed *= speedMultiplier;
        jumpForce *= speedMultiplier;
        dashSpeed *= speedMultiplier;
        animator.speed *= speedMultiplier;
        for (var i = 0; i < attackMovement.Length; i++)
        {
            attackMovement[i] *= speedMultiplier;
        }
        
        yield return new WaitForSeconds(duration);
        
        moveSpeed = originalMoveSpeed;
        jumpForce = originalJumpForce;
        dashSpeed = originalDashSpeed;
        animator.speed = originalAnimSpeed;
        for (var i = 0; i < attackMovement.Length; i++)
        {
            attackMovement[i] = originalAttackMovement[i];
        }
    }

    public IEnumerator BusyFor(float _seconds)
    {
        isBusy = true;
        yield return new WaitForSeconds(_seconds);
        isBusy = false;
    }
    private void CheckForDashInput()
    {
        if(isDead)
            return;
        
        if (IsWallDetected())
            return;
        
        if (Input.GetKeyDown(KeyCode.LeftShift) && SkillManager.Instance.dashSkill.CanUseSkill())
        {
            dashDirection = Input.GetAxisRaw("Horizontal");
            if (dashDirection == 0)
            {
                dashDirection = facingDirection;
            }
            playerStateMachine.ChangeState(dashState);
        }
    }
    
    public void AnimationTrigger() => playerStateMachine.currentState.AnimationFinishTrigger();

    public void AssignSword(GameObject _newSword)
    {
        sword = _newSword;
    }

    public void CatchTheSword()
    {
        playerStateMachine.ChangeState(catchSwordState);
        Destroy(sword);
    }

    protected override void EntityDeath()
    {
        base.EntityDeath();
        OnPlayerDeath?.Invoke();
        playerStateMachine.ChangeState(deathState);
    }
    
    public void ResetJumps()
    {
        currentJumpCount = 0;
    }

    private void OpenSkillTree()
    { 
        uiManager.ToggleSkillTreeUI();
    }

    public void TeleportPlayer(Vector3 position) => transform.position = position;
}
