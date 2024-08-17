using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScalePowerUI : MonoBehaviour
{
    [SerializeField] PlayerScalePower player;
    [SerializeField] Image barFront;
    [SerializeField] Color shrinkColor;
    [SerializeField] Color growColor;

    void Start()
    {
        player.OnUpdateScalePoints += PlayerScalePower_OnUpdateScalePoints;
    }

    private void PlayerScalePower_OnUpdateScalePoints(float newValuePercent)
    {
        barFront.fillAmount = newValuePercent;
        barFront.color = Color.Lerp(shrinkColor, growColor, newValuePercent);
    }
}
