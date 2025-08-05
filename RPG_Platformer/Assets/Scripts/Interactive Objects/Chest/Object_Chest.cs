using UnityEngine;

public class Object_Chest : MonoBehaviour, IDamageable
{
    private Animator _animator => GetComponentInChildren<Animator>();
    private Rigidbody2D _rigidbody => GetComponentInChildren<Rigidbody2D>();
    private Entity_VFX entityVFX => GetComponent<Entity_VFX>();
    private Entity_DropManager DropManager => GetComponent<Entity_DropManager>();

    [SerializeField] private Vector2 knockBack;
    [SerializeField] private bool canDropItems = true;
    public bool TakeDamage(float damage, float elementalDamage, ElementType elementType, Transform position, Transform damageDealer, bool isCritical)
    {
        if (canDropItems == false)
            return false;
        
        entityVFX.StartCoroutine(entityVFX.FlashFX());
        _animator.SetBool("chestOpen", true);
        DropManager.DropItems();
        _rigidbody.velocity = knockBack;
        _rigidbody.angularVelocity = Random.Range(-200f, 200f);
        
        return true;
    }
}
