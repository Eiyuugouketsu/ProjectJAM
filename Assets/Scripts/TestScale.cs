using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TestScale : MonoBehaviour
{
    [SerializeField] float scaleSpeed;
    [SerializeField] float minimumScale;
    [SerializeField] Rigidbody rb;
    float baseMass;
    float baseScale;
    float scaleValue = 0;
    public List<GameObject> touchingObjects = new List<GameObject>();
    public List<TestScale> scalableObjects => touchingObjects.Where(obj => obj.layer == LayerMask.NameToLayer("ScalableObject")).Select(obj => obj.GetComponent<TestScale>()).ToList();
    bool touchingCeiling => touchingObjects.Any(obj => obj.gameObject.layer == LayerMask.NameToLayer("Ceiling"));
    float touchingWalls => touchingObjects.Count(obj => obj.gameObject.layer == LayerMask.NameToLayer("Wall"));

    private void OnCollisionEnter(Collision other) 
    {
        touchingObjects.Add(other.gameObject);
    }

    private void OnCollisionExit(Collision other) 
    {
        touchingObjects.Remove(other.transform.gameObject);
    }
    private void Start() 
    {
        baseMass = rb.mass;
        baseScale = transform.localScale.x;
    }

    public void Grow(float growAmount)
    {
        if(CheckIfCanGrow())
        {
            scaleValue += growAmount * scaleSpeed;
        }
        else
        {
            Stop();
        }
        
    }
    public bool CheckIfCanGrow()
    {
        return (!(touchingWalls >1) && !touchingCeiling) && (!scalableObjects.Any(obj => obj.touchingWalls > 0 || obj.touchingCeiling)  || scalableObjects.Count == 0) && !PlayerThresholds.Instance.isCeilingAbove;
    }
    public bool CheckIfCanShrink()
    {
        return transform.localScale.x + scaleValue < minimumScale;
    }
    public void Shrink(float shrinkAmount)
    {
        scaleValue -= shrinkAmount * scaleSpeed;
    }

     public void Stop()
    {
        scaleValue = 0;
    }
    private void FixedUpdate() 
    {
        if(transform.localScale.x + scaleValue < minimumScale) return;
        transform.localScale = new Vector3(transform.localScale.x + scaleValue,transform.localScale.x + scaleValue,transform.localScale.x + scaleValue);
        rb.mass = Mathf.Pow(transform.localScale.x/baseScale,3)*baseMass;
    }

    public float GetMass()
    {
        return rb.mass;
    }

    public float GetMinimumScale()
    {
        return minimumScale;
    }
}
