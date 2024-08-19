using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    [SerializeField] public Transform TransitionBoxSpawn;
    [SerializeField] public Transform PlayerSpawnPoint;
    public void DisableAndDestroy()
    {
        //extra event unsubscribing goes here
        Destroy(gameObject);
    }
}
