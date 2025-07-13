using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class Buff
{
    public StatType type;
    public float value;
}
public class Object_Buff : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    private Entity_Stats _statsToModify;

    [Header("Buff Details")] 
    [SerializeField] private Buff[] buffs;
    [SerializeField] private string buffName;
    [SerializeField] private float buffDuration = 1f;
    [SerializeField] private bool canBeUsed;
    
    [Header("Floaty Movement")]
    [SerializeField] private float floatSpeed = 1f;
    [SerializeField] private float floatRange = 0.1f;
    private Vector3 _startPosition;

    private void Awake()
    {
        _startPosition = transform.position;
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void Update()
    {
        var yOffset = Mathf.Sin(Time.time * floatSpeed) * floatRange;
        transform.position = _startPosition + new Vector3(0, yOffset);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(canBeUsed == false)
            return;

        _statsToModify = collision.GetComponent<Entity_Stats>();
        StartCoroutine(BuffCoroutine(buffDuration));
    }

    private IEnumerator BuffCoroutine(float duration)
    {
        canBeUsed = false;
        _spriteRenderer.color = Color.clear;
        ApplyBuff(true);

        yield return new WaitForSeconds(duration);
        
        ApplyBuff(false);
        Destroy(gameObject);
    }

    private void ApplyBuff(bool apply)
    {
        foreach (var buff in buffs)
        {
            if(apply)
                _statsToModify.GetStatByType(buff.type).AddModifier(buff.value, buffName);
            else
                _statsToModify.GetStatByType(buff.type).RemoveModifier(buffName);
        }
    }
}
