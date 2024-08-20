using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsScrolling : MonoBehaviour
{
    public float scrollSpeed = 20f;
    public float stopPosition = 3000f;

    private RectTransform rectTransform;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        float bottomPosition = rectTransform.anchoredPosition.y - rectTransform.rect.height;

        if (bottomPosition < stopPosition)
        {
            rectTransform.anchoredPosition += Vector2.up * scrollSpeed * Time.deltaTime;
        }
        else
        {
            rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, stopPosition + rectTransform.rect.height);
        }

        
    }
}
