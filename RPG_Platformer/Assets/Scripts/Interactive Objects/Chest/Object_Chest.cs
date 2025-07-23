using UnityEngine;

public class Object_Chest : MonoBehaviour, IDamageable
{
    private Animator _animator => GetComponentInChildren<Animator>();
    private Rigidbody2D _rigidbody => GetComponentInChildren<Rigidbody2D>();
    private Entity_VFX entityVFX => GetComponent<Entity_VFX>();

    [SerializeField] private Vector2 knockBack;
    public bool TakeDamage(float damage, float elementalDamage, ElementType elementType, Transform position, Transform damageDealer, bool isCritical)
    {
        entityVFX.StartCoroutine(entityVFX.FlashFX());
        _animator.SetBool("chestOpen", true);
        _rigidbody.velocity = knockBack;
        _rigidbody.angularVelocity = Random.Range(-200f, 200f);
        
        //Drop Items
        return true;
    }
}
