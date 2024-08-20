using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DoorState{
    open,
    closed,
    damaged,
}
public class DoorController : MonoBehaviour
{

    private DoorState currDoorState;
    public DoorState CurrDoorState
    {
        get { return currDoorState; }
        set{
            currDoorState = value;
            UpdateDoorState(currDoorState);
        }
    }

    [SerializeField] Animator animator;
    //Optional future values
    private bool isKeyAligned;

    [Range(0.25f, 1.75f)]
    private float KeyMinSize; 
    [Range(0.25f, 1.75f)]
    private float KeyMaxSize; 
    public void UpdateDoorState(DoorState currDoorState)
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

    private void OpeningState()
    {
        animator.SetBool("IsOpened",true);
    }
    public void ClosingState()
    {
        animator.SetBool("IsOpened",false);
    }
}