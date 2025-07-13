using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Tooltip : MonoBehaviour
{
    private RectTransform _rectTransform;
    [SerializeField] private Vector2 offset = new Vector2(300, 20);
    
    protected virtual void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    public virtual void ShowToolTip(bool show, RectTransform targetRect)
    {
        if (show == false)
        {
            _rectTransform.position = new Vector2(9999, 9999);
            return;
        }
        
        UpdatePosition(targetRect);
    }

    private void UpdatePosition(RectTransform targetRect)
    {
        var screenCenterX = Screen.width / 2f;
        var screenTop = Screen.height;
        var screenBottom = 0f;
        Vector2 targetPosition = targetRect.position;

        targetPosition.x = targetPosition.x > screenCenterX ? targetPosition.x - offset.x : targetPosition.x + offset.x;
        var verticalHalf = _rectTransform.sizeDelta.y / 2f;
        var topY = targetPosition.y + verticalHalf;
        var bottomY = targetPosition.y - verticalHalf;

        if (topY > screenTop)
            targetPosition.y = screenTop - verticalHalf - offset.y;
        else if (bottomY < screenBottom)
            targetPosition.y = screenBottom + verticalHalf + offset.y;
        
        
        _rectTransform.position = targetPosition;
    }
    
    protected string GetColoredText(string color, string text)
    {
        return $"<color={color}>{text} </color>";
    }
}
