using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Manager : MonoBehaviour
{
    public UI_SkillToolTip skillTooltip;
    public UI_SkillTree skillTree;

    private bool _skillTreeEnabled;
    private void Awake()
    {
        skillTooltip = GetComponentInChildren<UI_SkillToolTip>();
        skillTree = GetComponentInChildren<UI_SkillTree>(true);
    }

    public void ToggleSkillTreeUI()
    {
        _skillTreeEnabled = !_skillTreeEnabled;
        skillTree.gameObject.SetActive(_skillTreeEnabled);
        skillTooltip.ShowToolTip(false, null);
    }
}
