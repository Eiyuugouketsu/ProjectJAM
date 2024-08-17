using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class KeyScript : MonoBehaviour
{
    private void OnTriggerEnter(Collider other){
        if(other.gameObject.GetComponent<DoorController>())
            other.gameObject.GetComponent<DoorController>().CurrDoorState = DoorController.DoorState.open;
    }

    private void OnTriggerExit(Collider other){
        if(other.gameObject.GetComponent<DoorController>())
            other.gameObject.GetComponent<DoorController>().CurrDoorState = DoorController.DoorState.closed;
    }
}
