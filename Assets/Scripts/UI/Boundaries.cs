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
        this.screenBounds = Camera.main.transform.position;
    }

    void LateUpdate()
    {

        // this.rb.MovePosition(viewPos);
        var cameraBottomBoundaryZ = Camera.main.transform.position.z + 10f;
        var cameraLeftBoundaryX = this.screenBounds.x - 25f;
        var cameraRightBoundaryX = this.screenBounds.x + 25f;
        
        var playerPositionZ = this.transform.position.z;
        var playerPositionX = this.transform.position.x;

        // Bound Z minimum position
        if(playerPositionZ < cameraBottomBoundaryZ)
        {
            playerPositionZ = cameraBottomBoundaryZ;
        }

        // Bound left and right X-Axis
        if (playerPositionX < cameraLeftBoundaryX) 
        {
            playerPositionX = cameraLeftBoundaryX;
        }

         if (playerPositionX > cameraRightBoundaryX){
            playerPositionX = cameraRightBoundaryX;
        }

        this.transform.position = new Vector3(playerPositionX, this.transform.position.y, playerPositionZ);
        
    }
}
