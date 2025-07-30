using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
public class Object_Buff : MonoBehaviour
{
    
    private Player_Stats _statsToModify;

    [Header("Buff Details")] 
    [SerializeField] private BuffEffectData[] buffs;
    [SerializeField] private string buffName;
    [SerializeField] private float buffDuration = 1f;
    
    [Header("Floaty Movement")]
    [SerializeField] private float floatSpeed = 1f;
    [SerializeField] private float floatRange = 0.1f;
    private Vector3 _startPosition;

    private void Awake()
    {
        _startPosition = transform.position;
    }

    private void Update()
    {
        var yOffset = Mathf.Sin(Time.time * floatSpeed) * floatRange;
        transform.position = _startPosition + new Vector3(0, yOffset);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        _statsToModify = collision.GetComponent<Player_Stats>();

        if (_statsToModify.CanApplyBuffOf(buffName))
        {
            _statsToModify.ApplyBuffOf(buffs, buffDuration, buffName);
            Destroy(gameObject);
        }
    }
}
