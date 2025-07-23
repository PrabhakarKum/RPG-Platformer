using System;
using UnityEngine;
using UnityEngine.Serialization;

public class SwordSkill : Skill_Base
{
   
   [Header("Sword Skill Info")] 
   [SerializeField] private Vector2 currentLaunchForce;
   [SerializeField] private float freezeTimeDuration = 0.7f;
   [SerializeField] private float returnSpeed = 12f;
   [SerializeField] private float maxAllowedDistance = 20f;
   
   [Header("Regular Sword Info")]
   [SerializeField] private GameObject swordPrefab;
   [SerializeField] private Vector2 regularLaunchForce;
   
   
   [Header("Bounce Info")]
   [SerializeField] private Vector2 bounceLaunchForce;
   [SerializeField] private int bounceAmount;
   [SerializeField] private float bouncingSpeed;
   
   [Header("Pierce Info")]
   [SerializeField] private Vector2 pierceLaunchForce;
   [SerializeField] private int pierceAmount;
   
   [Header("Spin Info")]
   [SerializeField] private Vector2 spinLaunchForce;
   [SerializeField] private float hitCooldown = 0.35f;
   [SerializeField] private int spinDuration = 2;
   [SerializeField] private float maxTravelDistance = 5f;


   private Vector2 finalDirection;
   
   [Header("Trajectory Prediction")]
   [SerializeField] private float swordGravity;
   [SerializeField] private int numberOfDots;
   [SerializeField] private float spaceBetweenDots;
   [SerializeField] private GameObject aimDotPrefab;
   [SerializeField] private Transform dotParent;
   
   private GameObject[] dots;
   
   protected override void Start()
   {
      base.Start();
      GenerateDots();
   }

   protected override void UseSkill()
   {
      UpdateThrowPower();
   }

   
   public void PredictTrajectory()
   {
      for (var i = 0; i < numberOfDots; i++)
      {
         dots[i].transform.position = DotsPosition(i * spaceBetweenDots);
      }
   }

   public void CreateSword()
   {
      var newSword = Instantiate(swordPrefab, dots[1].transform.position , Quaternion.identity);
      finalDirection = new Vector2(AimDirection().normalized.x * currentLaunchForce.x, AimDirection().normalized.y* currentLaunchForce.y);
      player.hasSword = false;
      var newSwordController =  newSword.GetComponent<SkillObject_Sword>();
      
      if(Unlocked(SkillUpgradeType.SwordThrow_Bounce))
         newSwordController.SetupBounce(true, bounceAmount, bouncingSpeed);
      
      if(Unlocked(SkillUpgradeType.SwordThrow_Pierce))
         newSwordController.SetupPierce(pierceAmount);
      
      if(Unlocked(SkillUpgradeType.SwordThrow_Spin))
         newSwordController.SetupSpinning(true, maxTravelDistance, spinDuration, hitCooldown);

      newSwordController.SetupSword(this, finalDirection, swordGravity, freezeTimeDuration, returnSpeed, maxAllowedDistance);
      
      player.AssignSword(newSword);
      
      DotsActive(false);
   }
   

   private void UpdateThrowPower()
   {
      Debug.Log("Updating throw power with upgrade: " + upgradeType);
      switch (upgradeType)
      {
         case SkillUpgradeType.SwordThrow:
            currentLaunchForce = regularLaunchForce; 
            break;
         
         case SkillUpgradeType.SwordThrow_Spin:
            currentLaunchForce = spinLaunchForce; 
            break;
         
         case SkillUpgradeType.SwordThrow_Bounce:
            currentLaunchForce = bounceLaunchForce;
            break;
         
         case SkillUpgradeType.SwordThrow_Pierce:
            currentLaunchForce = pierceLaunchForce; 
            break;
            
      }
   }

   #region Aim Sword

   private Vector2 AimDirection()
   {
      Vector2 playerPosition = player.transform.position;
      Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
      Vector2 direction = mousePosition - playerPosition;
      return direction;
   }
   public void DotsActive(bool enable)
   {
      foreach (var t in dots)
         t.SetActive(enable);
   }
   private void GenerateDots()
   {
      dots = new GameObject[numberOfDots];
      for (var i = 0; i < numberOfDots; i++)
      {
         dots[i] = Instantiate(aimDotPrefab, player.transform.position, Quaternion.identity, dotParent);
         dots[i].SetActive(false);
      }
   }
   private Vector2 DotsPosition(float t)
   {
      var gravityEffect = Physics2D.gravity * (0.5f * swordGravity * (t * t));
      
      var predictedPoint = new Vector2(AimDirection().normalized.x * currentLaunchForce.x, AimDirection().normalized.y * currentLaunchForce.y)* t + gravityEffect;

      var position = (Vector2)player.transform.root.position + predictedPoint;
      
     return position;
   }
   
   #endregion
}
