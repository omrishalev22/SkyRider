using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingWall : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Rigidbody>().velocity = new Vector3(5f, 0, 0);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("RightWall"))
        {
            GetComponent<Rigidbody>().velocity = new Vector3(-5f, 0, 0);
        }

        if (other.gameObject.CompareTag("LeftWall"))
        {
            GetComponent<Rigidbody>().velocity = new Vector3(5f, 0, 0);
        }
    }

}
