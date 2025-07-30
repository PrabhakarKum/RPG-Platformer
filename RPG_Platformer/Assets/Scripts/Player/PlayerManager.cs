using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;
    public Player player;
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(Instance.gameObject);    
        }
        else
        {
            Instance = this;
        }
    }
}
