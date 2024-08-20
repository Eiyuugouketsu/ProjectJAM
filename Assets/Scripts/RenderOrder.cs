using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderOrder : MonoBehaviour
{
    
    public Transform child1trans;
    public int child1index;
    public Transform child2trans;
    public int child2index;
    public Transform child3trans;
    public int child3index;
    public Transform child4trans;
    public int child4index = -1;

    private void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (child1index >= 0)
        {
            child1index = Mathf.Clamp(child1index, 0, transform.childCount - 1);
            child1trans.SetSiblingIndex(child1index);
        }

        if (child2index >= 0)
        {
            child2index = Mathf.Clamp(child2index, 0, transform.childCount - 1);
            child2trans.SetSiblingIndex(child2index);
        }

        if (child3index >= 0)
        {
            child3index = Mathf.Clamp(child3index, 0, transform.childCount - 1);
            child3trans.SetSiblingIndex(child3index);
        }

        if (child4index >= 0)
        {
            child4index = Mathf.Clamp(child4index, 0, transform.childCount - 1);
            child4trans.SetSiblingIndex(child4index);
        }
    }

}
