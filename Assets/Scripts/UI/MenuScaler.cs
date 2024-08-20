using UnityEngine;

public class MenuOptionScaler : MonoBehaviour
{
    public Transform flashlight;
    public RectTransform canvasRect;
    public float maxScale = 1.5f;
    public float minScale = 0.8f;
    public float maxDistance = 200f;

    private Vector3 initialScale;
    private RectTransform rectTransform;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        initialScale = rectTransform.localScale;
    }

    void Update()
    {
        Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(Camera.main, flashlight.position);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenPoint, null, out Vector2 localPoint);

        float distance = Vector2.Distance(localPoint, rectTransform.anchoredPosition);
        float scaleFactor = Mathf.Clamp(1 / (distance / maxDistance), minScale, maxScale);

        rectTransform.localScale = initialScale * scaleFactor;
    }
}