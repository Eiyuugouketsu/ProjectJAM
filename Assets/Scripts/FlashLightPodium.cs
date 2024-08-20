using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashLightPodium : ScalableObject
{
    public override void ObjectPickedUp(Transform holdPos)
    {
        PlayerThresholds.Instance.haveFlashlight = true;
        PlayerThresholds.Instance.PlayerGrabAbility.isHoldingObject = false;
        Destroy(gameObject);
    }
}
