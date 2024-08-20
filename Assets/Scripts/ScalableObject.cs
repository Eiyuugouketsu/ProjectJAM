using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ScalableObject : MonoBehaviour
{
    [SerializeField] float minimumScale;
    [SerializeField] protected Rigidbody rb;
    [SerializeField] OutlineManager outlineManager;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip[] collisionClips;
    [SerializeField] bool freezedUntillTouched;
    float baseMass;
    float baseScale;
    public List<GameObject> touchingObjects = new List<GameObject>();
    public List<ScalableObject> scalableObjects => touchingObjects.Where(obj => obj.layer == LayerMask.NameToLayer("ScalableObject")).Select(obj => obj.GetComponent<ScalableObject>()).ToList();
    bool touchingCeiling => touchingObjects.Any(obj => obj.gameObject.layer == LayerMask.NameToLayer("Ceiling"));
    bool touchingPlayerForceField => touchingObjects.Any(obj => obj.gameObject.layer == LayerMask.NameToLayer("PlayerForceField"));
    float touchingWalls => touchingObjects.Count(obj => obj.gameObject.layer == LayerMask.NameToLayer("Wall"));
    float destinationScale;
    Vector3 playerPosition;

    bool interactable = true;

    float collisionSoundTimeout = 1f;

    private void Awake()
    {
        baseScale = transform.localScale.x;
        destinationScale = transform.localScale.x;
        baseMass = rb.mass;
        if(freezedUntillTouched)
        {
            rb.constraints = RigidbodyConstraints.FreezeAll;
        }
    }

    private void OnCollisionEnter(Collision other) 
    {
        touchingObjects.Add(other.gameObject);
        PlayCollisionSound();
    }

    private void OnCollisionExit(Collision other) 
    {
        touchingObjects.Remove(other.transform.gameObject);
    }

    void PlayCollisionSound()
    {
        if (collisionSoundTimeout <= 0f)
        {
            audioSource.clip = collisionClips[Random.Range(0, collisionClips.Length)];
            audioSource.Play();
            collisionSoundTimeout = Random.Range(0f, 1f);
        }
    }

    public bool CheckIfCanGrow()
    {
        if (!interactable) return false;
        return (!(touchingWalls >1) && !touchingCeiling && !touchingPlayerForceField) && (!scalableObjects.Any(obj => obj.touchingWalls > 0 || obj.touchingCeiling)  || scalableObjects.Count == 0) && !PlayerThresholds.Instance.isCeilingAbove;
    }

    public bool CheckIfCanShrink()
    {
        if (!interactable) return false;
        return transform.localScale.x > minimumScale;
    }

    public void Grow(float growAmount, Vector3 playerPosition)
    {
        rb.constraints = RigidbodyConstraints.None;
        this.playerPosition = playerPosition;
        if (CheckIfCanGrow()) destinationScale = destinationScale + growAmount;

    }

    public void Shrink(float shrinkAmount, Vector3 playerPosition)
    {
        rb.constraints = RigidbodyConstraints.None;
        this.playerPosition = playerPosition;
        if (CheckIfCanShrink()) destinationScale = Mathf.Max(minimumScale, destinationScale - shrinkAmount);
    }

    void Update()
    {
        if (collisionSoundTimeout > 0) collisionSoundTimeout -= Time.deltaTime;
    }

    private void FixedUpdate() 
    {
        gameObject.layer = transform.localScale.x < destinationScale ? LayerMask.NameToLayer("CurrentlyGrowingScalableObject") : LayerMask.NameToLayer("ScalableObject");
        if (transform.localScale.x != destinationScale)
        {
            float scaleDiff = transform.localScale.x - destinationScale;
            transform.localScale = new Vector3(destinationScale, destinationScale, destinationScale);
            Vector3 pushAway = (playerPosition - transform.position).normalized * (scaleDiff / 2f);
            rb.velocity = new Vector3(pushAway.x, rb.velocity.y, pushAway.z);
            rb.mass = Mathf.Pow(transform.localScale.x/baseScale,3) * baseMass;
        }
    }

    private void LateUpdate()
    {
        if (transform.localScale.x == destinationScale) outlineManager.ChangeOutlineColor(ScaleState.None);
        else if (transform.localScale.x > destinationScale) outlineManager.ChangeOutlineColor(ScaleState.Shrinking);
        else if (transform.localScale.x < destinationScale) outlineManager.ChangeOutlineColor(ScaleState.Growing);
    }

    virtual public void SetIsKinematic(bool value)
    {
        rb.constraints = RigidbodyConstraints.None;
        rb.isKinematic = value;
    }

    public bool GetIsKinematic()
    {
        return rb.isKinematic;
    }

    public void ApplyForce(Vector3 holdPosForward)
    {
        rb.AddForce(holdPosForward * PlayerThresholds.Instance.getThrowForce(), ForceMode.Impulse);
    }

    public float GetMass()
    {
        return rb.mass;
    }

    public float GetMinimumScale()
    {
        return minimumScale;
    }

    public float GetCurrentScale()
    {
        return transform.localScale.x;
    }
    
    public bool GetIsInteractable()
    {
        return interactable;
    }

    public void SetIsInteractable(bool value)
    {
        interactable = value;
    }
}
