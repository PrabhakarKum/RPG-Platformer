using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Entity : MonoBehaviour
{
    [Header("KnockBack Info")]
    [SerializeField] protected Vector2 onDamageKnockBack;
    [SerializeField] protected float knockBackDuration;
    private bool _isKnocked;
    [Space]
    [Range(0, 1)] 
    [SerializeField] private float heavyDamageThreshold = 0.3f;
    [SerializeField] private float heavyKnockBackDuration = 0.5f;
    [SerializeField] private Vector2 onHeavyDamageKnockBack = new Vector2(3, 3); 
    
    [Header("Collision Info")] 
    public Transform attackCheck; 
    public float attackCheckRadius;
    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected float groundCheckDistance;
    [SerializeField] protected LayerMask groundLayer;
    [SerializeField] protected Transform wallCheck;
    [SerializeField] protected float wallCheckDistance;
    
    [Header("Health Info")]
    [SerializeField] protected float currentHp;
    [SerializeField] protected bool isDead = false;
    public int facingDirection  = 1;
    private bool _facingRight = true;

    [Header("Status Effect Details")] 
    [SerializeField] private float defaultDuration = 3;
    [SerializeField] private float chilledSlowMultiplier = 0.2f;

    private Coroutine _slowDownCo;
    
    #region Components
    
    private Slider healthBar;
    private Entity_Stats stats;
    private SpriteRenderer spriteRenderer;
    public Animator animator {get; private set;}
    public Rigidbody2D rigidBody {get; private set;}
    public Entity_VFX entityVFX {get; private set;}
    
    #endregion

    public event Action OnFlipped; 
    protected virtual void Awake()
    {
        entityVFX = GetComponent<Entity_VFX>();
        healthBar = GetComponentInChildren<Slider>();
        stats = GetComponent<Entity_Stats>();
    }

    protected virtual void Start()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponentInChildren<Animator>();
        rigidBody = GetComponent<Rigidbody2D>();
        currentHp = stats.GetMaxHealth();
        UpdateHealthBar();
    }

    protected virtual void Update()
    {
        
    }

    public virtual void TakeDamage(float damage, float elementalDamage, ElementType element, Transform target, Transform damageDealer, bool isCritical)
    {
        if(isDead)
            return;

        if (AttackEvaded())
        {
            Debug.Log($"{gameObject.name} attack evaded!!");
            return;
        }
        
        var attackerStats = damageDealer.GetComponent<Entity_Stats>();
        var armourReduction = attackerStats != null ? attackerStats.GetArmourReduction() : 0;
        
        var mitigation = stats.GetArmourMitigation(armourReduction);
        var physicalDamageTaken = damage * (1 - mitigation);

        var resistance = stats.GetElementalResistance(element);
        var elementalDamageTaken = elementalDamage * (1 - resistance);
        
        
        entityVFX.UpdateOnHitColor(element);
        entityVFX.CreateOnHitVFX(target, isCritical);
        entityVFX.StartCoroutine(entityVFX.FlashFX());
        StartCoroutine(HitKnockBack(physicalDamageTaken,damageDealer));
        ReduceHp(physicalDamageTaken + elementalDamageTaken);
    }

    public void ApplyStatusEffect(Transform target, ElementType element, float scaleFactor = 1f)
    {
        var statusHandler = target.GetComponent<Entity_StatusHandler>();
        if(statusHandler == null)
            return;

        if (element == ElementType.Ice && statusHandler.CanBeApplied(ElementType.Ice))
        {
            statusHandler.ApplyChilledEffect(defaultDuration, chilledSlowMultiplier);
        }
        else if (element == ElementType.Fire && statusHandler.CanBeApplied(ElementType.Fire))
        {
            var fireDamage = stats.offence.fireDamage.GetValue() * scaleFactor;
            statusHandler.ApplyBurnEffect(defaultDuration, fireDamage);
        }
    }

    public void ReduceHp(float damage)
    {
        currentHp -= damage;
        UpdateHealthBar();
        if (currentHp <= 0)
            Die();
    }

    private void UpdateHealthBar()
    {
        if(healthBar == null)
            return;
        
        healthBar.value = currentHp / stats.GetMaxHealth();
    }

    private void Die()
    {
        isDead = true;
        EntityDeath();
    }

    private IEnumerator HitKnockBack(float damage,Transform attacker)
    {
        _isKnocked = true;
        bool isHeavy = IsHeavyDamage(damage);
        
        var direction = CalculateKnockBack(isHeavy, attacker);
        rigidBody.velocity = new Vector2(direction.x, direction.y);
        
        float duration = isHeavy ? heavyKnockBackDuration : knockBackDuration;
        yield return new WaitForSeconds(duration);
        rigidBody.velocity = Vector2.zero;
        _isKnocked = false;
    }

    private Vector2 CalculateKnockBack(bool isHeavy, Transform attacker)
    {
        var direction = transform.position.x > attacker.position.x ? 1f : -1f;
        var knockBack = isHeavy ? onHeavyDamageKnockBack: onDamageKnockBack ;
        knockBack.x *= direction;
        return knockBack;
    }

    private bool IsHeavyDamage(float damage) => damage / stats.GetMaxHealth() > heavyDamageThreshold;
    
    private bool AttackEvaded() => Random.Range(0, 100) < stats.GetEvasion();

    protected virtual void EntityDeath()
    {
        
    }

    public virtual void SlowDownEntity(float duration, float slowMultiplier)
    {
        if(_slowDownCo != null)
            StopCoroutine(_slowDownCo);

        _slowDownCo = StartCoroutine(SlowDownEntityCoroutine(duration, slowMultiplier));
    }

    protected virtual IEnumerator SlowDownEntityCoroutine(float duration, float slowMultiplier)
    {
        yield return null;
    }
    
    #region Collision
   
    public bool IsGroundDetected() => Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, groundLayer);
    public bool IsWallDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right * facingDirection, wallCheckDistance, groundLayer);
    protected virtual void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
        Gizmos.color = Color.green;
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance * facingDirection, wallCheck.position.y));
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(attackCheck.position, attackCheckRadius);
    }
    #endregion
    
    #region Flip Controller
    public virtual void Flip()
    {
        facingDirection *= -1;
        _facingRight = !_facingRight;
        transform.Rotate(0f, 180f, 0f);
        
        OnFlipped?.Invoke();
    }

    public virtual void FlipController(float _x)
    {
        if (_x > 0 && !_facingRight)
        {
            Flip();
        }
        else if (_x < 0 && _facingRight)
        {
            Flip();
        }
    }
    #endregion
    
    #region Velocity
    public void SetZeroVelocity()
    {
        if(_isKnocked)
            return;
        
        rigidBody.velocity = new Vector2(0f, 0f);
    }

    public void SetVelocity(float xVelocity, float yVelocity)
    {
        if(_isKnocked)
            return;
        
        rigidBody.velocity = new Vector2(xVelocity, yVelocity);
        FlipController(xVelocity);
    }
    
    #endregion

    public void MakeTransparent(bool _isTransparent)
    {
        if (_isTransparent)
            spriteRenderer.color = Color.clear;
        else
            spriteRenderer.color = Color.white;
    }
}
