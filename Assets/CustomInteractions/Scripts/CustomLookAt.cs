using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomLookAt : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        var headPosition = Camera.main.transform.position;
        var lookPosition = headPosition - transform.position;
        lookPosition.y = 0;
        var rotation = Quaternion.LookRotation(lookPosition);
        transform.rotation = rotation;
    }
}