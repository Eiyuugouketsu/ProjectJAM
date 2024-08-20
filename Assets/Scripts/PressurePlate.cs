using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class PressurePlate : MonoBehaviour
{
    [SerializeField] float massNeeded;
    [SerializeField] DoorController doorController;
    List<ScalableObject> scalableObjects = new List<ScalableObject>();
    float yMax = 2f;
    float targetY;
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("ScalableObject"))
        {
            var obj = other.GetComponent<ScalableObject>();
            if(!scalableObjects.Contains(obj)) scalableObjects.Add(obj);
        }
    }
    void OnTriggerExit(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("ScalableObject"))
        {
            if(scalableObjects.Any(obj => obj.transform == other.transform))
            scalableObjects.Remove(scalableObjects.Where(obj => obj.transform == other.transform).First());
        }
    }
    void Update()
    {
        float sum = scalableObjects.Where(obj => !obj.GetIsKinematic() && ((Mathf.Abs(obj.transform.position.x) < Mathf.Abs(transform.position.x)+1) && (Mathf.Abs(obj.transform.position.x) > Mathf.Abs(transform.position.x)-1) && (Mathf.Abs(obj.transform.position.z) < Mathf.Abs(transform.position.z)+1) && (Mathf.Abs(obj.transform.position.z) > Mathf.Abs(transform.position.z)-1))).Sum(obj => obj.GetMass());
        if(sum>=massNeeded)
        {
            OpenDoor();
        }
        targetY = Mathf.Lerp(yMax,0,sum/massNeeded);
        Vector3 newPos = new Vector3(transform.localPosition.x, Mathf.Lerp(transform.localPosition.y,targetY,0.2f), transform.localPosition.z);
        transform.localPosition = newPos;
    }

    void OpenDoor()
    {
        doorController.UpdateDoorState(DoorState.open);
    }
}
