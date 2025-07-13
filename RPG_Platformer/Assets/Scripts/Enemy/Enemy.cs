using System.Collections;
using UnityEngine;
using UnityEngine.UI;
public class Enemy : Entity, IDamageable
{
    [SerializeField] protected LayerMask whatIsPlayer;
    public bool isPlayerDead;
    
    [Header("Move Info")] 
    public float moveSpeed;
    public float idleTime;
    private float defaultMoveSpeed;

    [Header("Attack Info")] 
    public float attackDistance;
    public float attackCooldown;
    public float battleTime;
    [HideInInspector] public float lastTimeAttacked;
    
    [Header("Stunned Info")] 
    public float stunDuration;
    public Vector2 stunDirection;
    [SerializeField] private bool _canBeStunned;
    
    public GameObject counterImage;
    [HideInInspector] public float flipThreshHold = 0.1f;
    

    protected EnemyStateMachine enemyStateMachine { get; private set; }
    
    protected override void Awake()
    {
        base.Awake();
        enemyStateMachine = new EnemyStateMachine();
        defaultMoveSpeed = moveSpeed;
    }

    protected override void Update()
    {
        base.Update();
        enemyStateMachine.currentState.Update();
    }

    protected override IEnumerator SlowDownEntityCoroutine(float duration, float slowMultiplier)
    {
        var originalMoveSpeed = moveSpeed;
        var originalAnimSpeed = animator.speed;
        var speedMultiplier = 1 - slowMultiplier;
        
        Debug.Log("enemy speed multiplier: "+ speedMultiplier);
        moveSpeed *= speedMultiplier;
        animator.speed *= speedMultiplier;
        
        yield return new WaitForSeconds(duration);

        moveSpeed = originalMoveSpeed;
        animator.speed = originalAnimSpeed;
    }
    public void FreezeTime(bool _timeFrozen)
    {
        if (_timeFrozen)
        {
            moveSpeed = 0;
            animator.speed = 0;
        }
        else
        {
            moveSpeed = defaultMoveSpeed;
            animator.speed = 1;
        }
    }

    protected IEnumerator FreezeTimerFor(float _seconds)
    {
        FreezeTime(true);
        yield return new WaitForSeconds(_seconds);
        FreezeTime(false);
    }

    #region Counter Attack Window
    public void OpenCounterAttackWindow()
    {
        if(isDead)
            return;
        
        _canBeStunned = true;
        counterImage.SetActive(true);
    }

    public void CloseCounterAttackWindow()
    {
        _canBeStunned = false;
        counterImage.SetActive(false);
    }
    #endregion

    public virtual bool CanBeStunned()
    {
        if (!_canBeStunned) return false;
        CloseCounterAttackWindow();
        return true;

    }

    public void AnimationTrigger() => enemyStateMachine.currentState.AnimationFinishTrigger();

    public RaycastHit2D IsPlayerDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right * facingDirection, 20, whatIsPlayer);

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x + attackDistance * facingDirection, transform.position.y));
    }

    public bool EnemyDead()
    {
        return isDead;
    }
    
    
}
