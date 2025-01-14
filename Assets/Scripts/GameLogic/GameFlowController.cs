﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameFlowController : MonoBehaviour
{
    public Transform tileObject;
    public Transform pickUps;
    public Transform blockingWallObj1;
    public Transform blockingWallObj2;
    public Transform rockObj;
    public Transform rock3Obj;
    public Text LevelText;
    public Text ScoreText;

    private Vector3 nextTileSpawn;
    private Vector3 nextPickUpSpwan;
    private Vector3 nextBlockingWall1;
    private Vector3 nextBlockingWall2;
    private Vector3 nextrockObj;
    private Vector3 nextrock3Obj;

    private bool isGameOver = false;
    private bool isGameRunning = true;
    private int level = 0;
    private int score = 0;

    private const int TileOffset = 90;
    private const int GameLevelInterval = 15; // increase level every X seconds
    private float CameraOffset = 100f; // create new tile every X     
    private float timeElapsedFromGameOver = 0f;
    private float delayBeforeMovingSceneOnGameFailure = 2f;

    List<GameObject> gameObjectsList = new List<GameObject>();

    void Start()
    {
        InitGame();
    }

    void Update()
    {
        if (isGameOver && isGameRunning)
        {
            timeElapsedFromGameOver += Time.deltaTime;
            if (timeElapsedFromGameOver > delayBeforeMovingSceneOnGameFailure)
            {
                SceneManager.LoadScene(2);
                isGameRunning = false;
            }
        }
    }

    private void InitGame()
    {
        nextTileSpawn.z = TileOffset;

        // start spwaning new tiles recuresivly
        StartCoroutine(SpawnTile());

        // start spwaning game level recuresivly
        StartCoroutine(ChangeGameLevel());

        // Starts the timer
        TimerController.instance.BeginTimer();

        isGameOver = false;
        isGameRunning = true;
        score = 0;
        level = 1;
        timeElapsedFromGameOver = 0;
    }

    public void IncreaseScore()
    {
        score += 1;
        ScoreText.text = $"Score: {score}";
    }

    public void SetGameOver()
    {
        StopAllCoroutines();
        TimerController.instance.EndTimer();
        ScoresController.instance.AddHighscoreEntry(score, TimerController.instance.GetTime());
        isGameOver = true;
    }

    public bool GetGameOver()
    {
        return isGameOver;
    }

    private int GetBoardSize()
    {
        return Convert.ToInt32(this.tileObject.GetComponent<MeshFilter>().mesh.bounds.size.x * this.tileObject.localScale.x);
    }

    IEnumerator ChangeGameLevel()
    {
        if (!this.isGameOver)
        {
            this.level++;
            this.LevelText.text = $"Level {level}";
            this.CameraOffset = Camera.main.GetComponent<CameraMoveController>().cameraVelocity * 6; // increase tile change rate
            this.IncreaseGameSpeed();
        }
        
        yield return new WaitForSeconds(GameLevelInterval); 
        StartCoroutine(ChangeGameLevel());
    }

    IEnumerator SpawnTile()
    {
        yield return new WaitForSeconds(1);
        var randomNum = UnityEngine.Random.Range(0, 10); // random number selection for randomization

        // create new objects only when camera gets closer to its end
        if (Camera.main.transform.position.z + this.CameraOffset >= nextTileSpawn.z)
        {
            GenerateRandomWalls(randomNum);
            GenerateRandomFallingRocks(randomNum);

            GenerateObjectRandomly(pickUps, nextPickUpSpwan, pickUps.rotation, -10, 6);

            InstantiateNewGameObject(tileObject, nextTileSpawn, tileObject.rotation);
            nextTileSpawn.z += tileObject.transform.localScale.z * tileObject.position.z;

            StartCoroutine(SpawnTile());
        }

        DestroyUnseenObjects(); // clean initiation of objects that are not under the camera view
        StartCoroutine(SpawnTile());
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
