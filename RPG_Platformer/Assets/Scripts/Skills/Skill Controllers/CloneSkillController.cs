using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class CloneSkillController : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private float cloneTimer;
    private Animator animator;
    [SerializeField] private float colorLoosingSpeed;
    
    [SerializeField] private Transform attackCheck;
    [SerializeField] private float attackCheckRadius;
    private Transform closestEnemy;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        cloneTimer -= Time.deltaTime;
        if (cloneTimer < 0)
            spriteRenderer.color = new Color(1f, 1f, 1f, spriteRenderer.color.a -(Time.deltaTime * colorLoosingSpeed));
        

        if (spriteRenderer.color.a <= 0)
            Destroy(gameObject);
        
    }

    public void SetupClone(Transform _newPosition, float _newCloneDuration, bool _canAttack, Vector3 _offset)
    {
        if (_canAttack)
            animator.SetInteger("AttackNumber", Random.Range(1, 4));
        
        transform.position = _newPosition.position + _offset;
        cloneTimer = _newCloneDuration;
        FaceClosestTarget();
    }
    
    private void AnimationTriggers()
    {
        
    }

    private void AttackTriggers(float damage)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(attackCheck.position, attackCheckRadius);
        foreach (var hit in colliders)
        {
            if(hit.GetComponent<Enemy>() != null)
                hit.GetComponent<Enemy>().TakeDamage(damage, hit.transform, transform);
        }
    }

    private void FaceClosestTarget()
    {
        Collider2D[] collider = Physics2D.OverlapCircleAll(transform.position, 25);
        float closestDistance = Mathf.Infinity;
        
        foreach (var hit in collider)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                float distanceToEnemy = Vector3.Distance(transform.position, hit.transform.position);
                if (distanceToEnemy < closestDistance)
                {
                    closestDistance = distanceToEnemy;
                    closestEnemy = hit.transform;
                }
            }
        }

        if (closestEnemy != null)
        {
            if (transform.position.x > closestEnemy.position.x)
            {
                transform.Rotate(0,180,0);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(attackCheck.position, attackCheckRadius);
        
    }
}
