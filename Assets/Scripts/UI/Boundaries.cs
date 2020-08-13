using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boundaries : MonoBehaviour
{
    private Vector3 screenBounds;
    Rigidbody rb;

    void Start()
    {
        this.rb = transform.GetComponent<Rigidbody>();
    }

    void LateUpdate()
    {
        Vector3 viewPos = transform.position;
        screenBounds = Camera.main.transform.position;
        viewPos.x = Mathf.Clamp(viewPos.x, -25f, 25f);
        viewPos.z = Mathf.Clamp(viewPos.z, screenBounds.z + 10f, screenBounds.z + 100f );
       
        this.rb.MovePosition(viewPos);
        this.rb.AddForce(0, 0, 10f);
        this.rb.velocity = new Vector3(0, 0, 50f);
    }
}
