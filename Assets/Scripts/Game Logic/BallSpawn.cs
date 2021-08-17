using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BallSpawn : MonoBehaviour
{
    [Header ("Important positions")]
    [SerializeField] private Transform spawnPosition;
    [SerializeField] private Transform waitingPosition;
    [SerializeField] private Transform loadPosition;

    [Header("What to spawn")]
    [SerializeField] private GameObject Ball;

    [Header("Time Values")]
    [SerializeField] private float timeToWaitSpot;

    [Header("Player state")]
    [SerializeField] private PlayerControls Player; 
    private GameObject waitingBall; 

    private void Start()
    {
        StartingBall();
        SpawnNewBall();
    }

    public void StartingBall()
    {
        GameObject startingBall = Instantiate(Ball, loadPosition.position, Quaternion.identity);
        Player.loadedBall = startingBall;
        Player.isLoaded = true;
    }

    public void SpawnNewBall()
    {
        GameObject newBall = Instantiate(Ball, spawnPosition.position, Quaternion.identity);
        TransferToWait(newBall);
    }

    void TransferToWait(GameObject newBall)
    {
        newBall.transform.DOMove(waitingPosition.position, timeToWaitSpot, false);
        waitingBall = newBall;
    }

    public void TransferToCannon()
    {
        waitingBall.transform.DOMove(loadPosition.position, timeToWaitSpot, false);
        Player.loadedBall = waitingBall;
        StartCoroutine("confirmLoaded");
    }

    private IEnumerator confirmLoaded()
    {
        yield return new WaitForSeconds(timeToWaitSpot);
        Player.isLoaded = true;
    } 
}
