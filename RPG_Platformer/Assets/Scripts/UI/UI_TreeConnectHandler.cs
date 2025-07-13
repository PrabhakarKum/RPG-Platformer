using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class UI_TreeConnectDetails
{
   public UI_TreeConnectHandler childNode;  // Which child node is connected at the end of this line
   public NodeDirectionType direction;
   [Range(100f, 350f)] public float length; // How long the line should be
   [Range(-25f, 25f)] public float rotation;
}
public class UI_TreeConnectHandler : MonoBehaviour
{
   [SerializeField] private UI_TreeConnection[] connections;
   [SerializeField] private UI_TreeConnectDetails[] connectionDetails;
   private Image _connectionImage;
   private Color _originalColor;

   private RectTransform _rectTransform => GetComponent<RectTransform>();

   private void Awake()
   {
      if (_connectionImage != null)
         _originalColor = _connectionImage.color;
   }

   public UI_TreeNode[] GetChildNodes()
   {
      var childrenToReturn = new List<UI_TreeNode>();

      foreach (var node in connectionDetails)
      {
         if (node.childNode != null)
         {
            childrenToReturn.Add(node.childNode.GetComponent<UI_TreeNode>());
         }
      }

      return childrenToReturn.ToArray();
   }

   private void OnValidate()
   {
      if(connectionDetails.Length <= 0)
         return;
      
      if (connectionDetails.Length != connections.Length)
      {
         Debug.LogWarning("Amount of details should be same as amount of connection "+ gameObject.name);
         return;
      }
      
      UpdateConnections();
   }

   private void UpdateConnections()
   {
      for (var i = 0; i < connectionDetails.Length; i++)
      {
         var detail = connectionDetails[i];
         var connection = connections[i];
         var targetPosition = connection.GetConnectionPoint(_rectTransform);

         Image connectionImage = connection.GetConnectionImage();
         
         connection.DirectConnection(detail.direction, detail.length, detail.rotation);
         
         if(detail.childNode == null)
            continue;
         
         detail.childNode.SetPosition(targetPosition);
         detail.childNode.SetConnectionImage(connectionImage);
         detail.childNode.transform.SetAsLastSibling();
      }
   }

   public void UpdateAllConnections()
   {
      UpdateConnections();
      foreach (var node in connectionDetails)
      {
         if(node.childNode == null)
            continue;
         
         node.childNode.UpdateConnections();
      }
   }

   public void UnlockConnectionImage(bool unlocked)
   {
      if(_connectionImage == null)
         return;

      _connectionImage.color = unlocked ? Color.white : _originalColor;
   }
   private void SetConnectionImage(Image image) => _connectionImage = image;
   private void SetPosition(Vector2 position) => _rectTransform.anchoredPosition = position;
}
