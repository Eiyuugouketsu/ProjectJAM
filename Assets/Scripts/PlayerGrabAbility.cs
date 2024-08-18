using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerGrabAbility : MonoBehaviour
{
    public event Action<ScalableObject> OnObjectPickedUp;
    public event Action OnObjectDropped;

    private PlayerMode playerMode;
    private PlayerRaycast playerRaycast;
    ScalableObject currObject;
    public bool isHoldingObject = false;
    [SerializeField] private Transform holdPos;

    // Start is called before the first frame update
    void Start()
    {
        playerMode = GetComponent<PlayerMode>();
        playerRaycast = GetComponent<PlayerRaycast>();

        if (playerRaycast != null)
        {
            playerRaycast.OnMouseOverScalableObject += HandleMouseOverScalableObject;
        }
    }

    private void HandleMouseOverScalableObject(ScalableObject scalableObject)
    {

        if (scalableObject != null)
        {
            currObject = scalableObject;
            //Debug.Log("Mouse is over a scalable object: " + scalableObject.name);
        }
        else
        {
            currObject = null;
            //Debug.Log("Mouse is not over any scalable object.");
        }
    }

    public void OnPickUpDrop()
    {
        if (playerMode.GetPlayerState() == PlayerState.Scale) return;
        if (isHoldingObject)
        {
            DropObject();
        }
        else if (currObject != null && currObject.GetMass() <= PlayerThresholds.Instance.getMaxCarryMass()) { 
            GrabObject();
        }
    }

    public void OnThrow()
    {
        if (isHoldingObject && currObject.GetMass() <= PlayerThresholds.Instance.GetMaxThrowMass())
        {
            ThrowObject();
        }
    }

    private void GrabObject()
    {
        isHoldingObject = true;

        currObject.SetIsKinematic(true);

        currObject.transform.position = holdPos.position;
        currObject.transform.SetParent(holdPos);
        OnObjectPickedUp(currObject);
    }

    private void DropObject()
    {
        isHoldingObject = false;

        currObject.SetIsKinematic(false);

        currObject.transform.SetParent(null);
        OnObjectDropped?.Invoke();
    }

    private void ThrowObject()
    {
        isHoldingObject = false;

        currObject.SetIsKinematic(true);
        currObject.ApplyForce(holdPos.forward);

        currObject.transform.SetParent(null);
        OnObjectDropped?.Invoke();
    }

    public ScalableObject GetCurrentObject()
    {
        return currObject;
    }

    public bool GetIsHoldingObject()
    {
        return isHoldingObject;
    }
}
