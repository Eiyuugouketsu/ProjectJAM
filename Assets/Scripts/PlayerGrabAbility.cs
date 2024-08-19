using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerGrabAbility : MonoBehaviour
{
    public event Action<ScalableObject> OnObjectPickedUp;
    public event Action OnObjectDropped;
    private PlayerRaycast playerRaycast;
    ScalableObject currObject;
    ScalableObject grabbedObject;
    public bool isHoldingObject = false;
    [SerializeField] private Transform holdPos;
    [SerializeField] float throwForce;

    // Start is called before the first frame update
    private void OnEnable() 
    {
        PlayerThresholds.Instance.PlayerRaycast.OnMouseOverGrabbableObject += HandleMouseOverGrabbableObject; 
    }

    void Update()
    {
        if (isHoldingObject && currObject != null && !currObject.GetIsInteractable())
        {
            DropObject();
        }
    }

    private void HandleMouseOverGrabbableObject(ScalableObject grabbableObject)
    {

        if (grabbableObject != null)
        {
            currObject = grabbableObject;
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
        if (PlayerThresholds.Instance.PlayerMode.GetPlayerState() == PlayerState.Scale) return;
        if (isHoldingObject)
        {
            DropObject();
        }
        else if (currObject != null && currObject.GetMass() <= PlayerThresholds.Instance.getMaxCarryMass() && currObject.GetIsInteractable()) { 
            GrabObject();
        }
    }

    public void OnThrow()
    {
        // Debug.Log($"isHoldingObject: {isHoldingObject} currNull: {currObject != null} currmass: {currObject != null && currObject.GetMass() <= PlayerThresholds.Instance.GetMaxThrowMass()}");
        if (isHoldingObject && grabbedObject != null && grabbedObject.GetMass() <= PlayerThresholds.Instance.GetMaxThrowMass())
        {
            ThrowObject();
        }
    }

    private void GrabObject()
    {
        isHoldingObject = true;
        grabbedObject = currObject;

        grabbedObject.SetIsKinematic(true);
        grabbedObject.transform.position = holdPos.position;
        grabbedObject.transform.SetParent(holdPos);
        OnObjectPickedUp?.Invoke(grabbedObject);
    }

    private void DropObject()
    {
        isHoldingObject = false;

        grabbedObject.SetIsKinematic(false);

        grabbedObject.transform.SetParent(null);
        grabbedObject = null;

        OnObjectDropped?.Invoke();
    }

    private void ThrowObject()
    {
        isHoldingObject = false;

        grabbedObject.SetIsKinematic(false);
        grabbedObject.transform.SetParent(null);
        grabbedObject.ApplyForce(holdPos.forward.normalized * throwForce);
        OnObjectDropped?.Invoke();
    }

    public ScalableObject GetCurrentObject()
    {
        return grabbedObject;
    }

    public bool GetIsHoldingObject()
    {
        return isHoldingObject;
    }
}
