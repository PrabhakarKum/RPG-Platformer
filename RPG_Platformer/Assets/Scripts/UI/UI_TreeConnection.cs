using UnityEngine;
using UnityEngine.UI;

public class UI_TreeConnection : MonoBehaviour
{
    // This script handles the actual drawing of the line from a node to its child.
    
    [SerializeField] private RectTransform rotationPoint;  // The pivot point for rotation (direction)
    [SerializeField] private RectTransform connectionLength; // The part that stretches (length)
    [SerializeField] private RectTransform childNodeConnectionPoint;  // The exact point where the child node should appear
    public void DirectConnection(NodeDirectionType directionType, float length, float offsetRotation)
    {
        var shouldBeActive = directionType != NodeDirectionType.None;

        float finalLength = shouldBeActive ? length : 0;
        float angle = GetDirectionAngle(directionType);
        
        rotationPoint.localRotation = Quaternion.Euler(0f, 0f, angle + offsetRotation);
        connectionLength.sizeDelta = new Vector2(finalLength, connectionLength.sizeDelta.y);
    }

    public Image GetConnectionImage() => connectionLength.GetComponent<Image>();

    public Vector2 GetConnectionPoint(RectTransform rectTransform)
    {
        //  It converts screen/world position into UI local position.
        RectTransformUtility.ScreenPointToLocalPointInRectangle
        (
            rectTransform.parent as RectTransform,
            childNodeConnectionPoint.position,
            null,
            out var localPosition

        );

        return localPosition;
    }
    
    private float GetDirectionAngle(NodeDirectionType directionType)
    {
        switch (directionType)
        {
            case NodeDirectionType.TopLeft: return 135f;
            case NodeDirectionType.TopCentre: return 90f;
            case NodeDirectionType.TopRight: return 45f;
            case NodeDirectionType.Left: return 180f;
            case NodeDirectionType.Right: return 0f;
            case NodeDirectionType.BottomLeft: return -135f;
            case NodeDirectionType.BottomCentre: return -90f;
            case NodeDirectionType.BottomRight: return -45f;
            default: return 0f;
            
        }
    }
}
public enum  NodeDirectionType
{
    None,
    TopLeft,
    TopCentre,
    TopRight,
    Left,
    Right,
    BottomLeft,
    BottomCentre,
    BottomRight
}
