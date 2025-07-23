using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class SkillObject_Clone : SkillObject_Base
{
    private static readonly int CanAttack = Animator.StringToHash("canAttack");
    
    [SerializeField] private GameObject onDeathVFX;
    [SerializeField] private float wispMoveSpeed = 0.05f;
    private float cloneTimer;
    public int maxAttacks;
    private bool shouldMoveToPlayer;

    private Animator animator;
    private Rigidbody2D rb;
    private Transform closestEnemy;
    private CloneSkill cloneManager;
    private Transform playerTransform;
    private TrailRenderer wispTrail;
    private SkillManager skillManager;
    private Entity_StatusHandler statusHandler;
    private SkillObject_Health cloneHealth;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if(shouldMoveToPlayer)
            HandleWispMovement();
        
    }

    public void SetupClone(CloneSkill _cloneManager,Transform _newPosition, Vector3 _offset)
    {
        cloneManager = _cloneManager;
        player = _cloneManager.player;
        playerTransform = _cloneManager.player.transform;
        playerStats = _cloneManager.player.entityStats;
        damageScaleData = _cloneManager.damageScaleData;
        cloneTimer = _cloneManager.cloneDuration;
        skillManager = _cloneManager.skillManager;
        statusHandler = _cloneManager.player.statusHandler;
        maxAttacks = cloneManager.GetMaxAttacks();

        animator.SetBool(CanAttack, maxAttacks > 0);
        transform.position = _newPosition.position + _offset;

        FaceClosestTarget();
        Invoke(nameof(HandleDeath), cloneTimer + 0.7f);

        cloneHealth = GetComponent<SkillObject_Health>();
        wispTrail = GetComponentInChildren<TrailRenderer>();
        wispTrail.gameObject.SetActive(false);
    }
    
    public void PerformAttack()
    {
        DamageEnemiesInRadius(targetCheck, checkRadius);

        if(targetGotHit == false)
            return;
        
        bool canDuplicate = Random.value < cloneManager.GetDuplicateChance();
        float xOffset = transform.position.x < lastTarget.position.x ? 0.2f : -0.2f;
        
        if(canDuplicate)
            cloneManager.CreateClone(lastTarget, new Vector2(xOffset, 0));
    }

    private void HandleWispMovement()
    {
        transform.position =
            Vector2.MoveTowards(transform.position, playerTransform.position, wispMoveSpeed * Time.deltaTime);

        if (Vector2.Distance(transform.position, playerTransform.position) < 0.1f)
        {
            HandlePlayerTouch();
            Destroy(gameObject);
        }
            
    }

    private void HandlePlayerTouch()
    {
        var healAmount = cloneHealth.lastDamageTaken * cloneManager.GetPercentOfDamageHealed();
        Debug.Log("heal amount: "+healAmount);
        player.IncreaseHealth(healAmount);

        var amountInSeconds = cloneManager.GetCoolReduceInSeconds();
        skillManager.ReduceAllSkillCooldownBy(amountInSeconds);
        
        if(cloneManager.CanRemoveNegativeEffects())
            statusHandler.RemoveAllNegativeEffects();
            
    }

    private void FaceClosestTarget()
    {
        closestEnemy = FindClosestTarget();

        if (closestEnemy != null && transform.position.x > closestEnemy.position.x)
        { 
            transform.Rotate(0,180,0);
        }
    }

    public void HandleDeath()
    {
        Instantiate(onDeathVFX, transform.position, Quaternion.identity);
        if (cloneManager.ShouldBeWisp())
        {
            TurnIntoWisp();
        }
        else
            Destroy(gameObject);
    }

    private void TurnIntoWisp()
    {
        shouldMoveToPlayer = true;
        animator.gameObject.SetActive(false);
        wispTrail.gameObject.SetActive(true);
        rb.simulated = false;
    }
}
