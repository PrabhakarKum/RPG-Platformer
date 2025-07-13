using UnityEngine;

public class VFX_AutoController : MonoBehaviour
{
   [SerializeField] private bool autoDestroy = true;
   [SerializeField] private float destroyDelay = 1;

   [SerializeField] private bool randomOffset = true;
   [SerializeField] private bool randomRotation = true;

   [Header("Random Rotation")] 
   [SerializeField] private float minRotation = 0f;
   [SerializeField] private float maxRotation = 360f;
   
   [Header("Random Position")] 
   [SerializeField] private float xMinOffset = -0.3f;
   [SerializeField] private float xMaxOffset = 0.3f;
   [Space]
   [SerializeField] private float yMinOffset = -0.3f;
   [SerializeField] private float yMaxOffset = 0.3f;
   private void Start()
   {
      ApplyRandomOffset();
      ApplyRandomRotation();
      
      if (autoDestroy)
         Destroy(gameObject, destroyDelay);
   }

   private void ApplyRandomOffset()
   {
      if(randomOffset == false)
         return;

      var xOffset = Random.Range(xMinOffset, xMaxOffset);
      var yOffset = Random.Range(yMinOffset, yMaxOffset);

      transform.position += new Vector3(xOffset, yOffset);
   }
   
   private void ApplyRandomRotation()
   {
      if(randomRotation == false)
         return;

      var zRotation = Random.Range(minRotation, maxRotation);

      transform.Rotate(0,0, zRotation);
   }
}
