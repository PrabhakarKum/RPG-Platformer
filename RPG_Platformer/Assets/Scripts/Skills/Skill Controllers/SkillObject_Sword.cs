using System.Collections.Generic;
using UnityEngine;
public class SkillObject_Sword : SkillObject_Base
{
   private Animator animator;
   private Rigidbody2D _rigidbody2D;
   private CircleCollider2D circleCollider;
   
   private bool canRotate = true;
   private bool isReturning;
   private float returnSpeed;
   private float maxAllowedDistance;
   

   private float freezeTimeDuration;
   
   [Header("Pierce Info")]
   private int pierceAmount;

   [Header("Spin Info")] 
   private float maxTravelDistance;
   private float spinDuration;
   private float spinTimer;
   private bool wasStopped;
   private bool isSpinning;
   
   private float hitTimer;
   private float hitCooldown;
   
   private float spinDirection;
   
   [Header("Bounce Info")]
   private float bouncingSpeed = 20f;
   private bool isBouncing;
   private int bounceAmount;
   private List<Transform> enemyTargets;
   public int targetIndex;
   
   private void Awake()
   {
      animator = GetComponentInChildren<Animator>();
      _rigidbody2D = GetComponent<Rigidbody2D>();
      circleCollider = GetComponent<CircleCollider2D>();
   }

   public void SetupSword(SwordSkill _swordSkill ,Vector2 _direction, float _gravityScale, float _FreezeTimeDuration, float _returnSpeed, float _maxAllowedDistance)
   {
      player = _swordSkill.player;
      playerStats = _swordSkill.player.entityStats;
      damageScaleData = _swordSkill.damageScaleData;
      
      freezeTimeDuration = _FreezeTimeDuration;
      _rigidbody2D.velocity = _direction;
      _rigidbody2D.gravityScale = _gravityScale;
      
      returnSpeed = _returnSpeed;
      maxAllowedDistance = _maxAllowedDistance;
      
      if(pierceAmount <= 0)
         animator.SetBool("Spinning", true);
      
      spinDirection = Mathf.Clamp(_rigidbody2D.velocity.x, -0.001f, 0.001f);
   }

   public void SetupBounce(bool _isBouncing, int _amountOfBounces, float _bouncingSpeed)
   {
      isBouncing = _isBouncing;
      bounceAmount = _amountOfBounces;
      enemyTargets = new List<Transform>();
      bouncingSpeed = _bouncingSpeed;
   }

   public void SetupPierce(int _pierceAmount)
   {
      pierceAmount = _pierceAmount;
   }

   public void SetupSpinning(bool _isSpinning, float _maxTravelDistance, int _spinDuration, float _hitCooldown)
   {
      isSpinning = _isSpinning;
      maxTravelDistance = _maxTravelDistance;
      spinDuration = _spinDuration;
      hitCooldown = _hitCooldown;
      
   }
   public void ReturnSword()
   {
      animator.SetBool("Spinning", true);
      _rigidbody2D.constraints = RigidbodyConstraints2D.FreezeAll;
      transform.parent = null;
      isReturning = true;
   }

   private void Update()
   {
      if(canRotate)
         transform.right = _rigidbody2D.velocity;

      HandleSwordComeback();
      SpinLogic();
      BounceLogic();
   }

   private void HandleSwordComeback()
   {
      float distance = Vector2.Distance(transform.position, player.transform.position);
      if (distance > maxAllowedDistance)
         isReturning = true;
      
      if (!isReturning) return;
      
      transform.position = Vector2.MoveTowards(transform.position, player.transform.position, returnSpeed * Time.deltaTime);
      if (distance < 0.15f)
      {
         animator.SetBool("Spinning", false);
         player.hasSword = true;
         player.CatchTheSword();
      }
   }

   private void SpinLogic()
   {
      if (!isSpinning) return;
      if(Vector2.Distance(playerStats.transform.position, transform.position) > maxTravelDistance && !wasStopped)
      {
         StopSpinning();
      }

      if (!wasStopped) return;
      spinTimer -= Time.deltaTime;
      transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x + spinDirection, transform.position.y ), 1.5f * Time.deltaTime);
            
      if (spinTimer <= 0)
      {
         isReturning = true;
         isSpinning = false;
      }
            
      hitTimer -= Time.deltaTime;
      if (!(hitTimer < 0)) return;
      hitTimer = hitCooldown;
      Collider2D[] enemiesAround =  GetEnemiesAround(transform, 0.1f);;
      foreach (var hit in enemiesAround)
      {
         if (hit != null)
            SwordSkillDamage(hit.GetComponent<Enemy>(), 0.5f);
      }
   }
   
   private void BounceLogic()
   {
      if (isBouncing && enemyTargets.Count > 0)
      {
         transform.position = Vector2.MoveTowards(transform.position, enemyTargets[targetIndex].position, bouncingSpeed * Time.deltaTime);
         if (Vector2.Distance(transform.position, enemyTargets[targetIndex].position) < 0.1f)
         {
            Debug.Log("bounce amount: "+ bounceAmount);
            SwordSkillDamage(enemyTargets[targetIndex].GetComponent<Enemy>(), 0.7f);
            targetIndex++;
            bounceAmount--;
            if (bounceAmount == 0)
            {
               isBouncing = false;
               isReturning = true;
            }
              
            if(targetIndex >= enemyTargets.Count)
               targetIndex = 0;
         }
      }
   }
   
   private void StopSpinning()
   {
      wasStopped = true;
      _rigidbody2D.constraints = RigidbodyConstraints2D.FreezePosition;
      spinTimer = spinDuration;
   }

   private void OnTriggerEnter2D(Collider2D collision)
   {
      if(isReturning)
         return;

      if (collision.GetComponent<Enemy>() != null)
      {
         var enemy = collision.GetComponent<Enemy>();
         SwordSkillDamage(enemy, 0.5f);
      }
      
      SetupTargetsForBounce(collision);
      StuckInto(collision);
   }

   private void SwordSkillDamage(Enemy enemy, float checkAttackRadius)
   {
      DamageEnemiesInRadius(enemy.transform , checkAttackRadius);
      enemy.StartCoroutine("FreezeTimerFor", freezeTimeDuration);
   }

   private void SetupTargetsForBounce(Collider2D collision)
   {
      if (collision.GetComponent<Enemy>() != null)
      {
         if (isBouncing && enemyTargets.Count <= 0)
         {
            Collider2D[] colliders = GetEnemiesAround(transform, 10);
            foreach (var enemy in colliders)
            {
               if (enemy != null&& enemy.gameObject != collision.gameObject)
                  enemyTargets.Add(enemy.transform);
            }
            
            enemyTargets.Add(collision.transform);
         }
      }
   }

   private void StuckInto(Collider2D collision)
   {
      if (pierceAmount > 0 && collision.GetComponent<Enemy>() != null)
      {
         pierceAmount--;
         return;
      }

      if (isSpinning)
      {
         StopSpinning();
         return;
      }
        
      
      canRotate = false;
      circleCollider.enabled = false;
      
      _rigidbody2D.isKinematic = true;
      _rigidbody2D.constraints = RigidbodyConstraints2D.FreezeAll;

      if (isBouncing && enemyTargets.Count > 0)
         return;
      
      animator.SetBool("Spinning", false);
      transform.parent = collision.transform;
   }
}
