using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Assertions.Comparers;

public class Player : Entity, IDamageable
{
    public static event Action OnPlayerDeath;

    [Header("Attack Details")] 
    public bool hasSword = true;
    public Vector2[] attackMovement;
    public float counterAttackDuration = 0.2f;
    public bool isBusy { get; private set; }

    [Header("Domain Expansion Details")] 
    public float riseSpeed = 10;
    public float riseMaxDistance = 0.5f;

    [Header("Movement Info")] public float moveSpeed = 10f;
    public float swordReturnImpact = 1.5f;

    [Header("Dash Info")] public float dashSpeed = 10f;
    public float dashDuration = 0.5f;
    public float dashDirection { get; private set; }

    [Header("Jump Info")] 
    public float jumpForce = 10f;
    public int maxJumps = 2;
    public int currentJumpCount = 0;
    public bool CanJump() => currentJumpCount < maxJumps;

    #region Components

    public PlayerAnimationTriggers playerCombat;
    public Player_Stats stats { get; private set; }
    public Player_VFX playerVFX { get; private set; }
    public SkillManager skillManager { get; private set; }
    public UI_Manager uiManager { get; private set; }
    public Entity_StatusHandler statusHandler { get; private set; }
    public GameObject sword { get; private set; }

    #endregion

    #region States

    public PlayerStateMachine playerStateMachine { get; private set; }
    public PlayerIdleState idleState { get; private set; }
    public PlayerMoveState moveState { get; private set; }
    public PlayerJumpState jumpState { get; private set; }
    public PlayerAirState airState { get; private set; }
    public PlayerDashState dashState { get; private set; }
    public PlayerWallSlideState wallSlideState { get; private set; }
    public PlayerWallJumpState wallJumpState { get; private set; }
    public PlayerPrimaryAttackState primaryAttackState { get; private set; }
    public PlayerCounterAttackState counterAttackState { get; private set; }
    public PlayerAimSwordState aimSwordState { get; private set; }
    public PlayerCatchSwordState catchSwordState { get; private set; }
    public PlayerDomainExpansionState domainExpansionState { get; private set;}
    public PlayerDeathState deathState {get; private set;}
    
    #endregion

    protected override void Awake()
    {
        base.Awake();

        uiManager = FindFirstObjectByType<UI_Manager>();
        stats = GetComponent<Player_Stats>();
        playerVFX = GetComponent<Player_VFX>();
        playerCombat = GetComponentInChildren<PlayerAnimationTriggers>();
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
        
        domainExpansionState = new PlayerDomainExpansionState(this, playerStateMachine, "Jump");
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
        CheckForDomainExpansionInput();
        
        if(Input.GetKeyDown(KeyCode.F))
            TryInteract();
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
        
        if(playerStateMachine.currentState == domainExpansionState)
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

    private void CheckForDomainExpansionInput()
    {
        if (Input.GetKeyDown(KeyCode.Alpha3) && SkillManager.Instance.domainExpansion.CanUseSkill())
        {
            if(isDead)
                return;
            
            if (SkillManager.Instance.domainExpansion.InstantDomain())
                SkillManager.Instance.domainExpansion.CreateDomain();
            else
                playerStateMachine.ChangeState(domainExpansionState);
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
    
    public void TeleportPlayer(Vector3 position) => transform.position = position;

    private void TryInteract()
    {
        Transform closest = null;
        var closestDistance = Mathf.Infinity;
        var objectsAround = Physics2D.OverlapCircleAll(transform.position + new Vector3(-0.01f, 0 ,0), 0.056f);

        foreach (var target in objectsAround)
        {
            var interactable = target.GetComponent<IInteractable>();
            if(interactable == null) continue;
            
            var distance = Vector2.Distance(transform.position, target.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closest = target.transform;
            }
        }
        
        if(closest == null)
            return;
        
        closest.GetComponent<IInteractable>().Interact();
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = new Color(1f, 0f, 0f, 0.3f);
    
        // Draw a wire disc at the object's position
        Gizmos.DrawWireSphere(transform.position + new Vector3(-0.01f, 0 ,0), 0.056f);
        
    }
}
