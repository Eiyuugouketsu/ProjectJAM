using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ScalableObject : MonoBehaviour
{
    [SerializeField] float scaleSpeed;
    [SerializeField] float minimumScale;
    [SerializeField] Rigidbody rb;
    float baseMass;
    float baseScale;
    public List<GameObject> touchingObjects = new List<GameObject>();
    public List<ScalableObject> scalableObjects => touchingObjects.Where(obj => obj.layer == LayerMask.NameToLayer("ScalableObject")).Select(obj => obj.GetComponent<ScalableObject>()).ToList();
    bool touchingCeiling => touchingObjects.Any(obj => obj.gameObject.layer == LayerMask.NameToLayer("Ceiling"));
    float touchingWalls => touchingObjects.Count(obj => obj.gameObject.layer == LayerMask.NameToLayer("Wall"));

    float destinationScale;

    private void Awake()
    {
        baseScale = transform.localScale.x;
        destinationScale = transform.localScale.x;
        baseMass = rb.mass;
    }

    private void OnCollisionEnter(Collision other) 
    {
        touchingObjects.Add(other.gameObject);
    }

    private void OnCollisionExit(Collision other) 
    {
        touchingObjects.Remove(other.transform.gameObject);
    }

    public bool CheckIfCanGrow()
    {
        return (!(touchingWalls >1) && !touchingCeiling) && (!scalableObjects.Any(obj => obj.touchingWalls > 0 || obj.touchingCeiling)  || scalableObjects.Count == 0) && !PlayerThresholds.Instance.isCeilingAbove;
    }

    public bool CheckIfCanShrink()
    {
        return transform.localScale.x > minimumScale;
    }

    public void Grow(float growAmount)
    {
        if (CheckIfCanGrow()) destinationScale = destinationScale + growAmount;
    }

    public void Shrink(float shrinkAmount)
    {
        if (CheckIfCanShrink()) destinationScale = Mathf.Max(minimumScale, destinationScale - shrinkAmount);
    }

    private void FixedUpdate() 
    {
        if (transform.localScale.x != destinationScale)
        {
            transform.localScale = new Vector3(destinationScale, destinationScale, destinationScale);
            rb.mass = Mathf.Pow(transform.localScale.x/baseScale,3) * baseMass;
        }
    }

    public void SetIsKinematic(bool value)
    {
        rb.isKinematic = value;
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
}
