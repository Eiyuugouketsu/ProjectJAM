using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerGrabAbility : MonoBehaviour
{
    public event Action<TestScale> OnObjectPickedUp;
    public event Action OnObjectDropped;

    private PlayerMode playerMode;
    private PlayerRaycast playerRaycast;
    TestScale currObject;
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

    private void HandleMouseOverScalableObject(TestScale scalableObject)
    {

        if (scalableObject != null)
        {
            currObject = scalableObject;
            Debug.Log("Mouse is over a scalable object: " + scalableObject.name);
        }
        else
        {
            currObject = null;
            Debug.Log("Mouse is not over any scalable object.");
        }
    }

    public void OnPickUpDrop()
    {
        Debug.Log("pickup");
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

        Rigidbody objRb = currObject.GetComponent<Rigidbody>();

        if (objRb != null) 
        {
            objRb.isKinematic = true;
        }

        currObject.transform.position = holdPos.position;
        currObject.transform.SetParent(holdPos);
        OnObjectPickedUp(currObject);
    }

    private void DropObject()
    {
        isHoldingObject = false;

        Rigidbody objRb = currObject.GetComponent<Rigidbody>();

        if (objRb != null)
        {
            objRb.isKinematic = false;
        }

        currObject.transform.SetParent(null);
        OnObjectDropped?.Invoke();
    }

    private void ThrowObject()
    {
        isHoldingObject = false;

        Rigidbody objRb = currObject.GetComponent<Rigidbody>();

        if (objRb != null)
        {
            objRb.isKinematic = false;
            objRb.AddForce(holdPos.forward * PlayerThresholds.Instance.getThrowForce(), ForceMode.Impulse);
        }

        currObject.transform.SetParent(null);
        OnObjectDropped?.Invoke();
    }

    public TestScale GetCurrentObject()
    {
        return currObject;
    }

    public bool GetIsHoldingObject()
    {
        return isHoldingObject;
    }
}
