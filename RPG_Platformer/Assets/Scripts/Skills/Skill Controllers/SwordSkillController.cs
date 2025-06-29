using System.Collections.Generic;
using UnityEngine;
public class SwordSkillController : MonoBehaviour
{
   private Animator _animator;
   private Rigidbody2D _rigidbody2D;
   private CircleCollider2D _circleCollider;
   private Player _player;
   
   private bool _canRotate = true;
   private bool _isReturning;
   private float _returnSpeed;

   private float _freezeTimeDuration;
   
   [Header("Pierce Info")]
   private int _pierceAmount;

   [Header("Spin Info")] 
   private float _maxTravelDistance;
   private float _spinDuration;
   private float _spinTimer;
   private bool _wasStopped;
   private bool _isSpinning;
   
   private float _hitTimer;
   private float _hitCooldown;
   
   private float _spinDirection;
   
   [Header("Bounce Info")]
   private float _bouncingSpeed = 20f;
   private bool _isBouncing;
   private int _bounceAmount;
   private List<Transform> _enemyTargets;
   private int _targetIndex;
   
   private void Awake()
   {
      _animator = GetComponentInChildren<Animator>();
      _rigidbody2D = GetComponent<Rigidbody2D>();
      _circleCollider = GetComponent<CircleCollider2D>();
   }

   public void SetupSword(Vector2 _direction, float _gravityScale, Player _player, float _FreezeTimeDuration, float _returnSpeed)
   {
      this._player = _player;
      _freezeTimeDuration = _FreezeTimeDuration;
      
      _rigidbody2D.velocity = _direction;
      _rigidbody2D.gravityScale = _gravityScale;
      
      this._returnSpeed = _returnSpeed;
      
      if(_pierceAmount <= 0)
         _animator.SetBool("Spinning", true);
      
      _spinDirection = Mathf.Clamp(_rigidbody2D.velocity.x, -0.001f, 0.001f);
   }

   public void SetupBounce(bool _isBouncing, int _amountOfBounces, float _bouncingSpeed)
   {
      this._isBouncing = _isBouncing;
      _bounceAmount = _amountOfBounces;
      _enemyTargets = new List<Transform>();
      this._bouncingSpeed = _bouncingSpeed;
   }

   public void SetupPierce(int _pierceAmount)
   {
      this._pierceAmount = _pierceAmount;
   }

   public void SetupSpinning(bool _isSpinning, float _maxTravelDistance, int _spinDuration, float _hitCooldown)
   {
      this._isSpinning = _isSpinning;
      this._maxTravelDistance = _maxTravelDistance;
      this._spinDuration = _spinDuration;
      this._hitCooldown = _hitCooldown;
      
   }
   public void ReturnSword()
   {
      _animator.SetBool("Spinning", true);
      _rigidbody2D.constraints = RigidbodyConstraints2D.FreezeAll;
      transform.parent = null;
      _isReturning = true;
   }

   private void Update()
   {
      if(_canRotate)
         transform.right = _rigidbody2D.velocity;

      if (_isReturning)
      {
         transform.position = Vector2.MoveTowards(transform.position, _player.transform.position, _returnSpeed * Time.deltaTime);
         if (Vector2.Distance(transform.position, _player.transform.position) < 0.15f)
         {
            _animator.SetBool("Spinning", false);
            _player.CatchTheSword();
         }
      }

      SpinLogic();
      BounceLogic();
   }

   private void SpinLogic()
   {
      if (_isSpinning)
      {
         if(Vector2.Distance(_player.transform.position, transform.position) > _maxTravelDistance && !_wasStopped)
         {
            StopSpinning();
         }

         if (_wasStopped)
         {
            _spinTimer -= Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x + _spinDirection, transform.position.y ), 1.5f * Time.deltaTime);
            
            if (_spinTimer <= 0)
            {
               _isReturning = true;
               _isSpinning = false;
            }
            
            _hitTimer -= Time.deltaTime;
            if (_hitTimer < 0)
            {
               _hitTimer = _hitCooldown;
               Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 0.1f);
               foreach (var hit in colliders)
               {
                  if (hit.GetComponent<Enemy>() != null)
                     SwordSkillDamage(hit.GetComponent<Enemy>(), hit.GetComponent<Enemy>().transform);
               }
            }
         }
      }
   }
   
   private void BounceLogic()
   {
      if (_isBouncing && _enemyTargets.Count > 0)
      {
         transform.position = Vector2.MoveTowards(transform.position, _enemyTargets[_targetIndex].position, _bouncingSpeed * Time.deltaTime);
         if (Vector2.Distance(transform.position, _enemyTargets[_targetIndex].position) < 0.1f)
         {
            SwordSkillDamage(_enemyTargets[_targetIndex].GetComponent<Enemy>(), _enemyTargets[_targetIndex].GetComponent<Enemy>().transform);
            _targetIndex++;
            _bounceAmount--;

            if (_bounceAmount == 0)
            {
               _isBouncing = false;
               _isReturning = true;
            }
              
            if(_targetIndex >= _enemyTargets.Count)
               _targetIndex = 0;
         }
      }
   }
   
   private void StopSpinning()
   {
      _wasStopped = true;
      _rigidbody2D.constraints = RigidbodyConstraints2D.FreezePosition;
      _spinTimer = _spinDuration;
   }

   private void OnTriggerEnter2D(Collider2D collision)
   {
      if(_isReturning)
         return;

      if (collision.GetComponent<Enemy>() != null)
      {
         Enemy enemy = collision.GetComponent<Enemy>();
         SwordSkillDamage(enemy, collision.GetComponent<Enemy>().transform);
      }
      
      SetupTargetsForBounce(collision);
      
      StuckInto(collision);
   }

   private void SwordSkillDamage(Enemy enemy, Transform position)
   {
      enemy.TakeDamage(1, position, transform);
      enemy.StartCoroutine("FreezeTimerFor", _freezeTimeDuration);
   }

   private void SetupTargetsForBounce(Collider2D collision)
   {
      if (collision.GetComponent<Enemy>() != null)
      {
         if (_isBouncing && _enemyTargets.Count <= 0)
         {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 10);
            foreach (var hit in colliders)
            {
               if (hit.GetComponent<Enemy>() != null)
                  _enemyTargets.Add(hit.transform);
            }
         }
      }
   }

   private void StuckInto(Collider2D collision)
   {
      if (_pierceAmount > 0 && collision.GetComponent<Enemy>() != null)
      {
         _pierceAmount--;
         return;
      }

      if (_isSpinning)
      {
         StopSpinning();
         return;
      }
        
      
      _canRotate = false;
      _circleCollider.enabled = false;
      
      _rigidbody2D.isKinematic = true;
      _rigidbody2D.constraints = RigidbodyConstraints2D.FreezeAll;

      if (_isBouncing && _enemyTargets.Count > 0)
         return;
      
      _animator.SetBool("Spinning", false);
      transform.parent = collision.transform;
   }
}
