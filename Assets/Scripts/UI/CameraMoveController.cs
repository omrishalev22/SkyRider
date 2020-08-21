using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMoveController : MonoBehaviour
{
    [SerializeField]
    public GameFlowController gameFlow;
    public float cameraVelocity = 20;
    private float lastVelocity;


    void Start()
    {
        this.GetComponent<Rigidbody>().velocity = new Vector3(0,0, cameraVelocity);
        this.lastVelocity = cameraVelocity;
    }

    void Update()
    {
        // 0 = game over
        if(cameraVelocity != lastVelocity && gameFlow && !gameFlow.GetGameOver())
        {
            GetComponent<Rigidbody>().velocity = new Vector3(0, 0, cameraVelocity);
        }
        
    }
}
