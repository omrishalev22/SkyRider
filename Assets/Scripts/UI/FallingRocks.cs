using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingRocks : MonoBehaviour
{
    public float fallingVelocity = -15f;
    void Start()
    {
        GetComponent<Rigidbody>().velocity = new Vector3(0, 0, fallingVelocity);
    }
}
