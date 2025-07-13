using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_SkillTree : MonoBehaviour
{
    [SerializeField] private int skillPoint;
    [SerializeField] private UI_TreeConnectHandler[] parentNodes;
    
    public SkillManager skillManager { get; private set;}

    [ContextMenu("Reset Skill Tree")]
    public void RefundAllSkills()
    {
        UI_TreeNode[] skillNodes = GetComponentsInChildren<UI_TreeNode>();

        foreach (var node in skillNodes)
        {
            node.Refund();
        }
    }
    public void RemoveSkillPoints(int cost) => skillPoint -= cost;
    public bool EnoughSkillPoints(int cost) => skillPoint >= cost;
    public void AddSkillPoints(int points) => skillPoint += points;

    private void Awake()
    {
        skillManager = SkillManager.Instance;
    }

    private void Start()
    {
        UpdateAllConnections();
    }

    [ContextMenu("Update All Connections")]
    public void UpdateAllConnections()
    {
        foreach (var node in parentNodes)
        {
            node.UpdateAllConnections();
        }
    }
}
