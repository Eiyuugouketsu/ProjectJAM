using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGrabAbility : MonoBehaviour
{
    TestScale currObject;
    private PlayerRaycast playerRaycast;
    public bool isHoldingObject = false;
    [SerializeField] private float maxMass = 100f;
    [SerializeField] private Transform holdPos;
    [SerializeField] private float holdDistance = 2f;
    [SerializeField] private float throwForce = 5f;
    // Start is called before the first frame update
    void Start()
    {
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

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) 
        {
            if (isHoldingObject)
            {
                DropObject();
            }
            else if (currObject != null && currObject.GetMass() <= maxMass) { 
                GrabObject();
            }
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
       
    }
}
