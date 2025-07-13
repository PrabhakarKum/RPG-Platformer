using System;
using UnityEngine;

public class SkillObject_Crystal : SkillObject_Base
{
    [SerializeField] private GameObject vfxPrefab;
    private Transform _target;
    private float _speed;
    private CrystalSkill crystalSkill;

    private void Update()
    {
        if(_target == null)
            return;
        
        transform.position = Vector3.MoveTowards(transform.position, _target.position, _speed * Time.deltaTime);
    }

    public void MoveTowardsClosestTarget(float speed)
    {
        _target = FindClosestTarget();
        _speed = speed;
    }
   
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.GetComponent<Enemy>() == null)
            return;

        Explode();
    }

    public void Explode()
    {
        DamageEnemiesInRadius(transform, checkRadius);
        Instantiate(vfxPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    public void SetupCrystal(CrystalSkill crystalSkill)
    {
        this.crystalSkill = crystalSkill;
        playerStats = crystalSkill.player.entityStats;
        damageScaleData = crystalSkill.damageScaleData;
        var detonationTime = crystalSkill.GetDetonateTime();
        
        Invoke(nameof(Explode), detonationTime);
    }

    public void SetupCrystal(CrystalSkill crystalSkill, float detonationTime, bool canMove, float crystalSpeed)
    {
        this.crystalSkill = crystalSkill;
        playerStats = crystalSkill.player.entityStats;
        damageScaleData = crystalSkill.damageScaleData;
        
        Invoke(nameof(Explode), detonationTime);
        
        if(canMove)
            MoveTowardsClosestTarget(crystalSpeed);
    }
}
