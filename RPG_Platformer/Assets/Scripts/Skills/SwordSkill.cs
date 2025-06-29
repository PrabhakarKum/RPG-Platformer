using UnityEngine;
using UnityEngine.Serialization;

public enum SwordType
{
   Regular,
   Bounce, 
   Pierce,
   Spinning
   
}
public class SwordSkill : Skill
{
   public SwordType swordType = SwordType.Regular;
   
   
   [Header("Bounce Info")]
   [SerializeField] private int bounceAmount;
   [SerializeField] private float bounceGravity;
   [SerializeField] private float bouncingSpeed;
   
   [Header("Pierce Info")]
   [SerializeField] private int pierceAmount;
   [SerializeField] private float pierceGravity;
   
   [Header("Spin Info")]
   [SerializeField] private float hitCooldown = 0.35f;
   [SerializeField] private int spinDuration = 2;
   [SerializeField] private float maxTravelDistance = 5f;
   [SerializeField] private float spinGravity = 1f;
   
   [Header("Sword Skill Info")]
   [SerializeField] private GameObject swordPrefab;
   [SerializeField] private Vector2 launchForce;
   [SerializeField] private float swordGravity;
   [SerializeField] private float freezeTimeDuration = 0.7f;
   [SerializeField] private float returnSpeed = 12f;

   private Vector2 finalDirection;
   
   [Header("Aim Dots")]
   [SerializeField] private int numberOfDots;
   [SerializeField] private float spaceBetweenDots;
   [SerializeField] private GameObject aimDotPrefab;
   [SerializeField] private Transform dotParent;
   
   private GameObject[] dots;
   
   protected override void Start()
   {
      base.Start();
      GenerateDots();

      SetupGravity();
   }

   private void SetupGravity()
   {
      if(swordType == SwordType.Bounce)
         swordGravity = bounceGravity;
      else if(swordType == SwordType.Pierce)
         swordGravity = pierceGravity;
      else if(swordType == SwordType.Spinning)
         swordGravity = spinGravity;
   }

   protected override void Update()
   {
      base.Update();
      
      if(Input.GetKey(KeyCode.Mouse1))
      {
         for (int i = 0; i < numberOfDots; i++)
         {
            dots[i].transform.position = DotsPosition(i * spaceBetweenDots);
         }
      }
   }
   public void CreateSword()
   {
      finalDirection = new Vector2(AimDirection().normalized.x * launchForce.x, AimDirection().normalized.y* launchForce.y);
      GameObject newSword = Instantiate(swordPrefab, player.transform.position, Quaternion.identity);
      SwordSkillController newSwordController =  newSword.GetComponent<SwordSkillController>();

      if (swordType == SwordType.Bounce)
      {
         newSwordController.SetupBounce(true, bounceAmount, bouncingSpeed);
      }
      else if (swordType == SwordType.Pierce)
      {
         newSwordController.SetupPierce(pierceAmount);
      }
      else if (swordType == SwordType.Spinning)
      {
         newSwordController.SetupSpinning(true, maxTravelDistance, spinDuration, hitCooldown);
      }
      
      newSwordController.SetupSword(finalDirection, swordGravity, player, freezeTimeDuration, returnSpeed);
      
      player.AssignSword(newSword);
      
      DotsActive(false);
   }

   #region Aim Sword
   public Vector2 AimDirection()
   {
      Vector2 playerPosition = player.transform.position;
      Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
      Vector2 direction = mousePosition - playerPosition;
      return direction;
   }
   public void DotsActive(bool _isActive)
   {
      for(int i = 0; i < dots.Length; i++)
         dots[i].SetActive(_isActive);
   }
   private void GenerateDots()
   {
      dots = new GameObject[numberOfDots];
      for (int i = 0; i < numberOfDots; i++)
      {
         dots[i] = Instantiate(aimDotPrefab, player.transform.position, Quaternion.identity, dotParent);
         dots[i].SetActive(false);
      }
   }
   private Vector2 DotsPosition(float t)
   {
     Vector2 position = (Vector2)player.transform.position + new Vector2(
        AimDirection().normalized.x * launchForce.x,
        AimDirection().normalized.y * launchForce.y)* t + 0.5f * (Physics2D.gravity * swordGravity) * (t*t);
         position.x = Mathf.Clamp(position.x, -10f + player.transform.position.x, 10f+  player.transform.position.x);
         position.y = Mathf.Clamp(position.y, -10f + player.transform.position.y, 10f +  player.transform.position.y);
        // Debug.Log("Player position" + player.transform.position + "dots position" + position);
        // Debug.Log("---------------------");
     return position;
   }
   #endregion
}
