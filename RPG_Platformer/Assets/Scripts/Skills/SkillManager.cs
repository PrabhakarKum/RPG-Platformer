using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public static SkillManager Instance;
    public DashSkill dashSkill {get; private set;}
    public CloneSkill cloneSkill {get; private set;}
    
    public SwordSkill swordSkill {get; private set;}
    
    public BlackHoleSkill blackHoleSkill {get; private set;}
    public CrystalSkill crystalSkill {get; private set;}

    private void Awake()
    {
        if (Instance != null)
            Destroy(Instance.gameObject);
        else
            Instance = this;
    }

    private void Start()
    {
        dashSkill = GetComponent<DashSkill>();
        cloneSkill = GetComponent<CloneSkill>();
        swordSkill = GetComponent<SwordSkill>();
        blackHoleSkill = GetComponent<BlackHoleSkill>();
        crystalSkill = GetComponent<CrystalSkill>();
    }

    public Skill_Base GetSkillByType(SkillType type)
    {
        switch (type)
        {
            case SkillType.DashSkill:
                return dashSkill;
            
            case SkillType.TimeCrystalSkill:
                return crystalSkill;
            
            default: 
                Debug.Log($"Skill type {type} not found");
                return null;
        }
    }
}
