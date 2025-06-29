using UnityEngine;

public class Chest : MonoBehaviour, IDamageable
{
    private Animator _animator => GetComponentInChildren<Animator>();
    private Rigidbody2D _rigidbody => GetComponentInChildren<Rigidbody2D>();
    private Entity_VFX EntityVFX => GetComponent<Entity_VFX>();

    [SerializeField] private Vector2 knockBack;
    public void TakeDamage(float damage, Transform position, Transform DamageDealer)
    {
        _animator.SetBool("chestOpen", true);
        _rigidbody.velocity = knockBack;
        _rigidbody.angularVelocity = Random.Range(-200f, 200f);
        EntityVFX.StartCoroutine("FlashFX");
        
        //Drop Items

    }
}
