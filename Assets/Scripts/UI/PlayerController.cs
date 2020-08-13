using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    public GameObject explosion;

    [SerializeField]
    public GameFlowController gameFlowController;

    public float speed;

    private Rigidbody rb;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = transform.GetComponent<Rigidbody>();
    }

    // FixedUpdate is called before applying physicts
    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        rb.velocity = movement * speed;
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Pick Up"))
        {
            other.gameObject.SetActive(false);
        }

        if (other.gameObject.CompareTag("Block"))
        {
            Camera.main.GetComponent<Rigidbody>().velocity = new Vector3(0,0,0); // stop camera from moving in case
            transform.gameObject.SetActive(false);
            Instantiate(explosion, transform.position, transform.rotation);
            this.gameFlowController.isGameOver = true;
        }
    }
}
