using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FlashLightPodium : ScalableObject
{
    public UnityEvent onObjectPickedUp;
    public override void ObjectPickedUp(Transform holdPos)
    {
        PlayerThresholds.Instance.haveFlashlight = true;
        PlayerThresholds.Instance.PlayerGrabAbility.isHoldingObject = false;
        PlayerThresholds.Instance.PlayerMode.OnChangeMode();
        onObjectPickedUp.Invoke();
        Destroy(gameObject);
    }
}
