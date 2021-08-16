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
    [SerializeField] private GameObject waitingBall; 

    private void Start()
    {
        SpawnNewBall();
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

        if (!Player.isLoaded)
        {
            TransferToCannon(waitingBall);
        }
    }

    void TransferToCannon(GameObject currentBall)
    {
        currentBall.transform.DOMove(loadPosition.position, timeToWaitSpot, false);
        Player.loadedBall = currentBall;
        StartCoroutine("confirmLoaded");
    }

    private IEnumerator confirmLoaded()
    {
        yield return new WaitForSeconds(timeToWaitSpot);
        Player.isLoaded = true;
    } 
}
