using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public static SkillManager Instance;
    public DashSkill dashSkill {get; private set;}
    public CloneSkill cloneSkill {get; private set;}
    
    public SwordSkill swordSkill {get; private set;}
    public CrystalSkill crystalSkill {get; private set;}

    private Skill_Base[] allSkills;
    
    public DomainExpansion domainExpansion { get; private set; }

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
        domainExpansion = GetComponent<DomainExpansion>();
        crystalSkill = GetComponent<CrystalSkill>();

        allSkills = GetComponents<Skill_Base>();
    }

    public void ReduceAllSkillCooldownBy(float amount)
    {
        foreach (var skill in allSkills)
            skill.ReduceCooldownBy(amount);
    }

    public Skill_Base GetSkillByType(SkillType type)
    {
        switch (type)
        {
            case SkillType.DashSkill:
                return dashSkill;
            
            case SkillType.TimeCrystalSkill:
                return crystalSkill;

            case SkillType.SwordSkill:
                return swordSkill;
            
            case SkillType.CloneSkill:
                return cloneSkill;
            
            case SkillType.DomainExpansionSkill:
                return domainExpansion;
            default: 
                Debug.Log($"Skill type {type} not found");
                return null;
        }
    }
}
