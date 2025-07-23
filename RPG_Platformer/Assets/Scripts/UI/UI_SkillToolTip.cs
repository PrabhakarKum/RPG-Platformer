using System.Collections;
using System.Text;
using TMPro;
using UnityEngine;

public class UI_SkillToolTip : UI_Tooltip
{
   private UI_Manager _uiManager;
   private UI_SkillTree _skillTree;
   
   [SerializeField] private TextMeshProUGUI skillName;
   [SerializeField] private TextMeshProUGUI skillDescription;
   [SerializeField] private TextMeshProUGUI skillCooldown;
   [SerializeField] private TextMeshProUGUI skillRequirements;

   [Space] 
   [SerializeField] private string metConditionHex;
   [SerializeField] private string notMetConditionHex;
   [SerializeField] private string importantInfoHex;
   [SerializeField] private string lockedSkillText = "You've taken a different path - this skill is now locked";
   [SerializeField] private Color exampleColor;

   private Coroutine _textEffectCoroutine;

   protected override void Awake()
   {
      base.Awake();
      _uiManager = GetComponentInParent<UI_Manager>();
      _skillTree = _uiManager.GetComponentInChildren<UI_SkillTree>(true);
   }

   public override void ShowToolTip(bool show, RectTransform targetRect)
   {
      base.ShowToolTip(show, targetRect);
   }

   public void ShowToolTip(bool show, RectTransform targetRect, UI_TreeNode node)
   {
      base.ShowToolTip(show, targetRect);
      
      if(show == false)
         return;

      skillName.text = node.skillData.displayName;
      skillDescription.text = node.skillData.description;
      skillCooldown.text = "Cooldown: " + node.skillData.upgradeData.cooldown + " s.";
      var skillLockedText = GetColoredText(importantInfoHex, lockedSkillText);
      var requirements = node.isLocked ? skillLockedText : GetRequirements(node.skillData.cost, node.neededNodes, node.conflictNodes);

      skillRequirements.text = requirements;
   }

   public void LockedSkillEffect()
   {
      if(_textEffectCoroutine != null)
         StopCoroutine(_textEffectCoroutine);

      _textEffectCoroutine = StartCoroutine(TextBlinkEffectCoroutine(skillRequirements, 0.15f, 3));

   }
   private IEnumerator TextBlinkEffectCoroutine(TextMeshProUGUI text, float blinkInterval, int blinkCount)
   {
      for (int i = 0; i < blinkCount; i++)
      {
         text.text = GetColoredText(notMetConditionHex, lockedSkillText);
         yield return new WaitForSeconds(blinkInterval);
         
         text.text = GetColoredText(importantInfoHex, lockedSkillText);
         yield return new WaitForSeconds(blinkInterval);
      }
   }

   private string GetRequirements(int skillCost, UI_TreeNode[] neededNodes, UI_TreeNode[] conflictNodes)
   {
      var stringBuilder = new StringBuilder();
      stringBuilder.AppendLine("Requirements: ");

      var costColor = _skillTree.EnoughSkillPoints(skillCost) ? metConditionHex : notMetConditionHex;
      var costText = $"- {skillCost} skill point(s)";
      var finalCostText = GetColoredText(costColor, costText);
      stringBuilder.AppendLine(finalCostText);

      foreach (var node in neededNodes)
      {
         if(node == null)
            continue;
         
         var nodeColor = node.isUnlocked ? metConditionHex : notMetConditionHex;
         var nodeText = $"- {node.skillData.displayName}";
         var finalNodeText = GetColoredText(nodeColor, nodeText);
         stringBuilder.AppendLine(finalNodeText);
      }
   
      if (conflictNodes.Length <= 0)
         return stringBuilder.ToString();

      stringBuilder.AppendLine(); // Spacing
      
      stringBuilder.AppendLine("Locks out: ");
      foreach (var node in conflictNodes)
      {
         if(node == null)
            continue;
         
         var nodeText = $"- {node.skillData.displayName}";
         var finalNodeText = GetColoredText(importantInfoHex, nodeText);
         stringBuilder.AppendLine(finalNodeText);
      }
      
      return stringBuilder.ToString();
   }

   
}
