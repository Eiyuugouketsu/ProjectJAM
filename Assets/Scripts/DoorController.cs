using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    public enum DoorState{
        open,
        closed,
        damaged,
    }

    private DoorState currDoorState;
    public DoorState CurrDoorState
    {
        get { return currDoorState; }
        set{
            currDoorState = value;
            UpdateDoorState(currDoorState);
        }
    }

    //Optional future values
    private bool isKeyAligned;

    [Range(0.25f, 1.75f)]
    private float KeyMinSize; 
    [Range(0.25f, 1.75f)]
    private float KeyMaxSize; 
    

    //Removable values(just for testing if logic works)
    [SerializeField]
    private GameObject doorLeft, doorRight;
    [SerializeField]
    private Transform openPosL , openPosR;
    private Vector3 posL , posR;
    void Start()
    {
        posL = doorLeft.transform.position;
        posR = doorRight.transform.position;
    }

    void UpdateDoorState(DoorState currDoorState)
    {
        switch (currDoorState)
        {
            case DoorState.open:
                OpeningState();
                break;
            case DoorState.closed:
                ClosingState();
                break;
            default:
                break;
        }
    }

    private void OpeningState(){
        //Opening Sequence
        doorLeft.transform.position = openPosL.position;
        doorRight.transform.position = openPosR.position;
    }
    private void ClosingState(){
        //Closing Sequence
        doorLeft.transform.position = posL;
        doorRight.transform.position = posR;
    }
}