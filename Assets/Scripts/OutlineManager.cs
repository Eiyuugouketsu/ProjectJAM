using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutlineManager : MonoBehaviour
{
    [SerializeField] Outline outline;

    [SerializeField] Color growColor;
    [SerializeField] Color shrinkColor;

    public void ToggleOutlineActive(bool value)
    {
        outline.enabled = value;
    }

    public void ChangeOutlineColor(ScaleState scaleState)
    {
        Color color = Color.white;
        if (scaleState == ScaleState.Growing) color = growColor;
        if (scaleState == ScaleState.Shrinking) color = shrinkColor;
        outline.OutlineColor = color;
    }
}
