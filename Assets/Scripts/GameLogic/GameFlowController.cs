using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameFlowController : MonoBehaviour
{
    public Transform tileObject;
    public Transform pickUps;
    public Transform blockingWallObj1;
    public Transform blockingWallObj2;
    public Transform rockObj;
    public Transform rock3Obj;
    public Canvas LevelTextCanvas;

    private Vector3 nextTileSpawn;
    private Vector3 nextPickUpSpwan;
    private Vector3 nextBlockingWall1;
    private Vector3 nextBlockingWall2;
    private Vector3 nextrockObj;
    private Vector3 nextrock3Obj;

    public bool isGameOver = false;
    private int level = 1;
    private (int min, int max) boardWidthDimensions;
    private (int min, int max) gameRightOutsideBoundary;
    private (int min, int max) gameLeftOutsideBoundary;

    private const int TileOffset = 90;
    private const int GameLevelInterval = 15; // increase level every X seconds
    private float CameraOffset = 100f; // create new tile every X     

    List<GameObject> gameObjectsList = new List<GameObject>();

    void Start()
    {
        InitGame();
    }

    void Update()
    {
        if (this.isGameOver)
        {
            StopAllCoroutines();
            TimerController.instance.EndTimer();
        }
    }

    private void InitGame()
    {
        nextTileSpawn.z = TileOffset;
        this.boardWidthDimensions = (this.getBoardSize()/2 * -1, this.getBoardSize()/2);

        // start spwaning new tiles recuresivly
        StartCoroutine(spawnTile());

        // start spwaning game level recuresivly
        StartCoroutine(ChangeGameLevel());

        // Starts the timer
        TimerController.instance.BeginTimer();
    }

    private int getBoardSize()
    {
        return Convert.ToInt32(this.tileObject.GetComponent<MeshFilter>().mesh.bounds.size.x * this.tileObject.localScale.x);
    }

    IEnumerator ChangeGameLevel()
    {
        if (!this.isGameOver)
        {
            this.LevelTextCanvas.GetComponentInChildren<Text>().text = $"Level {level}";
            this.level++;
            this.CameraOffset = Camera.main.GetComponent<CameraMoveController>().cameraVelocity * 6; // increase tile change rate
            this.IncreaseGameSpeed();
        }
        
        yield return new WaitForSeconds(GameLevelInterval); 
        StartCoroutine(ChangeGameLevel());
    }

    IEnumerator spawnTile()
    {
        yield return new WaitForSeconds(1);
        var randomNum = UnityEngine.Random.Range(0, 10); // random number selection for randomization

        // create new objects only when camera gets closer to its end
        Debug.Log($"camera: {Camera.main.transform.position.z + this.CameraOffset}, nexttile: {nextTileSpawn.z}");
        if (Camera.main.transform.position.z + this.CameraOffset >= nextTileSpawn.z)
        {
            GenerateRandomWalls(randomNum);
            GenerateRandomFallingRocks(randomNum);

            GenerateObjectRandomly(pickUps, nextPickUpSpwan, pickUps.rotation, -10, 6);

            InstantiateNewGameObject(tileObject, nextTileSpawn, tileObject.rotation);
            nextTileSpawn.z += tileObject.transform.localScale.z * tileObject.position.z;

            StartCoroutine(spawnTile());
        }

        DestroyUnseenObjects(); // clean initiation of objects that are not under the camera view
        StartCoroutine(spawnTile());
    }

    // destroy instantiated object that are not part of the screen anymore
    private void DestroyUnseenObjects() 
    {
        this.gameObjectsList.ForEach(gameObject =>
        {
            if (gameObject && Camera.main.transform.position.z >= gameObject.transform.position.z)
            {
                // destroy unseen elemetn after 1 second from outbound
                Destroy(gameObject, 1);
            }
        });
        
    }

    private void InstantiateNewGameObject(Transform original, Vector3 position, Quaternion q)
    {        
        var clone = Instantiate(original, position, q);
        // add element to list so it can later on be destroyed
        gameObjectsList.Add(clone.gameObject);
    }

    private void IncreaseGameSpeed()
    {
        Camera.main.GetComponent<CameraMoveController>().cameraVelocity = Camera.main.GetComponent<CameraMoveController>().cameraVelocity * 1.2f;
    }

    private void GenerateObjectRandomly(Transform original, Vector3 nextObjectPosition, Quaternion q, int minRange, int maxRange)
    {
        var randomX = UnityEngine.Random.Range(minRange, maxRange);
        nextObjectPosition.z = nextTileSpawn.z;
        nextObjectPosition.x = randomX;
        InstantiateNewGameObject(original, nextObjectPosition, q);
    }

    private void GenerateRandomWalls(int randomSelection)
    {
        if (randomSelection % 2 == 0)
        {
            GenerateObjectRandomly(blockingWallObj1, nextBlockingWall1, blockingWallObj1.rotation, -7, 7);
        }
        else
        {
            GenerateObjectRandomly(blockingWallObj2, nextBlockingWall2, blockingWallObj2.rotation, -4, 4);
        }
    }

    private void GenerateRandomFallingRocks(int randomSelection)
    {
        // just random logic to generate falling walls
        Transform gameObj = null;
        Vector3 mextGameObj = new Vector3(0,0,0);

        /*
         * 0 = left side
         * 1 = right side
         * 2 = both sides
         */
        switch (randomSelection % 3)
        {
            case 0:
                gameObj = randomSelection % 2 == 0 ? rockObj : rock3Obj;
                GenerateObjectRandomly(gameObj, mextGameObj, gameObj.rotation, -22, -12);
                break;
            case 1:
                gameObj = randomSelection % 2 == 0 ? rockObj : rock3Obj;
                GenerateObjectRandomly(gameObj, mextGameObj, gameObj.rotation, -22, -12);
                break;
            case 2:
                nextrockObj.z = nextrockObj.z * 4f;
                GenerateObjectRandomly(rockObj, nextrockObj, rockObj.rotation, -22, -12);
                GenerateObjectRandomly(rock3Obj, nextrock3Obj, rock3Obj.rotation, 13, 22);
                break;
        }

    }
}
