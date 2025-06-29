using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BlackHole_HotKey_Controller : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private TextMeshProUGUI myText;
    private KeyCode myHotKey;
    private Transform myEnemyTransform;
    private BlackHoleSkillController blackHole;
    public void SetHotKey(KeyCode _hotKey, Transform _myEnemy, BlackHoleSkillController _myBlackHole)
    {
        myText = GetComponentInChildren<TextMeshProUGUI>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        myEnemyTransform = _myEnemy;
        blackHole = _myBlackHole;
        
        myHotKey = _hotKey;
        myText.text = _hotKey.ToString();
    }

    private void Update()
    {
        if (Input.GetKeyDown(myHotKey))
        {
            blackHole.AddEnemiesToList(myEnemyTransform);
            myText.color = Color.clear;
            spriteRenderer.color = Color.clear;
        }
    }
}
