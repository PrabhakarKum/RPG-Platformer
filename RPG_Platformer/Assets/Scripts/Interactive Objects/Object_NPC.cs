using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_NPC : MonoBehaviour
{
    protected Transform player;
    protected UI_Manager uiManager;
    [SerializeField] private Transform npc;
    [SerializeField] private GameObject interactToolTip;
    private bool facingRight = true;
    
    [Header("Floaty Tooltip")]
    [SerializeField] private float floatSpeed = 8f;
    [SerializeField] private float floatRange = 0.1f;
    private Vector3 _startPosition;

    protected virtual void Awake()
    {
        uiManager = FindFirstObjectByType<UI_Manager>();
        _startPosition = interactToolTip.transform.position;
        interactToolTip.SetActive(false);
    }

    protected virtual void Update()
    {
        HandleNpcFlip();
        HandleToolTip();
    }

    private void HandleToolTip()
    {
        if (!interactToolTip.activeSelf) return;
        var yOffset = Mathf.Sin(Time.time * floatSpeed) * floatRange;
        interactToolTip.transform.position = _startPosition + new Vector3(0, yOffset);
    }

    private void HandleNpcFlip()
    {
        if(player == null || npc == null)
            return;

        if (npc.position.x > player.position.x && facingRight)
        {
            npc.transform.Rotate(0f, 180f, 0f);
            facingRight = false;
        }
        else if (npc.position.x < player.position.x && !facingRight)
        {
            npc.transform.Rotate(0f, 180f, 0f);
            facingRight = true;
        }
    }
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        player = collision.transform;
        interactToolTip.SetActive(true);
        
    }

    protected virtual void OnTriggerExit2D(Collider2D collision)
    {
        interactToolTip.SetActive(false);
    }
}
