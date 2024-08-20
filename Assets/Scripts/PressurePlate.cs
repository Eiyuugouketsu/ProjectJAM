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
    [SerializeField] float timeToSink;
    [SerializeField] Vector3 sunkPosition;
    [SerializeField] DoorController doorController;
    [SerializeField] AudioSource audioSource;
    List<ScalableObject> scalableObjects = new List<ScalableObject>();
    Vector3 startingPosition;
    float yMax = 2f;
    float targetY;

    float sinkTimer = 0f;
    bool audioPlaying = false;

    void Awake()
    {
        startingPosition = transform.localPosition;
    }

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
        float sum = scalableObjects.Where(obj => !obj.isBeingHeld && ((Mathf.Abs(obj.transform.position.x) < Mathf.Abs(transform.position.x)+1) && (Mathf.Abs(obj.transform.position.x) > Mathf.Abs(transform.position.x)-1) && (Mathf.Abs(obj.transform.position.z) < Mathf.Abs(transform.position.z)+1) && (Mathf.Abs(obj.transform.position.z) > Mathf.Abs(transform.position.z)-1))).Sum(obj => obj.GetMass());
        if(sum>=massNeeded && sinkTimer < timeToSink)
        {
            if (!audioPlaying)
            {
                audioSource.Play();
                audioPlaying = true;
            }
            sinkTimer += Time.deltaTime;
            if (sinkTimer >= timeToSink)
            {
                sinkTimer = timeToSink;
                OpenDoor();
            }
        } else if (sum < massNeeded && sinkTimer > 0f)
        {
            audioPlaying = false;
            sinkTimer -= Time.deltaTime;
            CloseDoor();
            if (sinkTimer < 0f) sinkTimer = 0f;
        }
        transform.localPosition = Vector3.Lerp(startingPosition, sunkPosition, sinkTimer / timeToSink);
    }

    void OpenDoor()
    {
        doorController.UpdateDoorState(DoorState.open);
    }

    void CloseDoor()
    {   
        doorController.UpdateDoorState(DoorState.closed);
    }
}
