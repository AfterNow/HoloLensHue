using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttractedToObject : MonoBehaviour
{

    public GameObject attractedTo;
    public float speed = 5.0f;
    public float distanceToSnapWithin = 0.01f;

    private Rigidbody rb;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (Vector3.Distance(attractedTo.transform.position, transform.position) < distanceToSnapWithin)
        {
            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, attractedTo.transform.position, step);
        }      
    }
}

