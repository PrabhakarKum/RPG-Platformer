using System.Collections;
using UnityEngine;
using UnityEngine.UI;
public class Enemy : Entity, IDamageable
{
    [SerializeField] protected LayerMask whatIsPlayer;
    public bool isPlayerDead;
    
    [Header("Move Info")] 
    public float moveSpeed;
    public float battleMoveSpeed;
    public float idleTime;

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
    private float activeSlowMultiplier;
    
    protected override void Awake()
    {
        base.Awake();
        enemyStateMachine = new EnemyStateMachine();
        activeSlowMultiplier = 1f;
    }

    protected override void Update()
    {
        base.Update();
        enemyStateMachine.currentState.Update();
    }

    public float GetMoveSpeed() => moveSpeed * activeSlowMultiplier * Time.deltaTime;

    public float GetBattleMoveSpeed() => battleMoveSpeed * activeSlowMultiplier * Time.deltaTime;

    protected override IEnumerator SlowDownEntityCoroutine(float duration, float slowMultiplier)
    {
        
        activeSlowMultiplier = 1 - slowMultiplier;
        
        
        animator.speed *= activeSlowMultiplier;
        
        yield return new WaitForSeconds(duration);
        StopSlowDown();
    }

    public override void StopSlowDown()
    {
        activeSlowMultiplier = 1;
        animator.speed = 1;
        base.StopSlowDown();
    }

    private void FreezeTime(bool _timeFrozen)
    {
        if (_timeFrozen)
        {
            activeSlowMultiplier = 0;
            animator.speed = 0;
        }
        else
        {
            activeSlowMultiplier = 1;
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

    public override bool TakeDamage(float damage, float elementalDamage, ElementType element, Transform target, Transform damageDealer, bool isCritical)
    {
        if (canTakeDamage == false)
            return false;

        var wasHit =  base.TakeDamage(damage, elementalDamage, element, target, damageDealer, isCritical);

        if (wasHit == false)
            return false;
        
        return true;
    }
}
