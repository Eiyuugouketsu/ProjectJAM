using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Keyhole : MonoBehaviour
{
    [Tooltip("A string for the key id. It only matches a key that has the same key id.")]
    [SerializeField] string keyId;

    [SerializeField] float minKeySize;
    [SerializeField] float maxKeySize;

    [SerializeField] Transform keySetTransform;

    [Tooltip("This is the distance in units that the key will snap to before sliding into the keySetTransform position.")]
    [SerializeField] float keySlideDistance;

    [Tooltip("This is the amount of time in seconds that the key animates sliding into the keyhole.")]
    [SerializeField] float keySlideTime;

    [SerializeField] DoorController doorController;

    float keySlideTimer = 0f;

    KeyScript keyCurrentlyInside;

    bool activated = false;

    public UnityEvent onUnlock;
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<KeyScript>())
        {
            keyCurrentlyInside = other.gameObject.GetComponent<KeyScript>();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<KeyScript>())
        {
            keyCurrentlyInside = null;
        }
    }

    void Update()
    {
        if (!activated && keyCurrentlyInside)
        {
            CheckIfKeyFits(keyCurrentlyInside);
        }
    }

    public void CheckIfKeyFits(KeyScript key)
    {
        if (key.GetKeyId() == keyId)
        {
            float keySize = key.transform.localScale.x;
            if (keySize >= minKeySize && keySize <= maxKeySize)
            {
                key.ConsumeKey();
                Activate(key);
            }
        }
    }

    void Activate(KeyScript key)
    {
        onUnlock?.Invoke();

        activated = true;
        Vector3 keyStartingPosition = keySetTransform.position + (transform.forward * -1f * keySlideDistance);
        key.transform.position = keyStartingPosition;
        key.transform.rotation = keySetTransform.rotation;
        StartCoroutine(SlideKey(key.transform));
    }

    void OpenDoor()
    {
        doorController.UpdateDoorState(DoorState.open);
    }

    IEnumerator SlideKey(Transform keyTransform)
    {
        Vector3 keyStartingPosition = keyTransform.position;
        while (keySlideTimer < keySlideTime)
        {
            keyTransform.position = Vector3.Lerp(keyStartingPosition, keySetTransform.position, Mathf.Min(keySlideTimer / keySlideTime, 1f));
            keySlideTimer += Time.deltaTime;
            yield return null;
        }
        keyTransform.position = keySetTransform.position;
        OpenDoor();
    }

}
