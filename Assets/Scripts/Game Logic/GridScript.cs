using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridScript : MonoBehaviour
{
    private Vector3 startPos;
    private Vector3 startRoof;
    private Vector3 heightToLose = new Vector3 (0f, 0.52f, 0f);

    public Transform[] gridPlace;
    public GameObject Roof;

    public float timeBetweenLowerings;

    [SerializeField] private AudioSource soundSpeaker;
    [SerializeField] private AudioClip[] soundClip;
    [SerializeField] private GameManager gameManager;

    private void Start()
    {
        startPos = transform.position;
        startRoof = Roof.transform.position;
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    private IEnumerator LowerGrid()
    {
        yield return new WaitForSeconds(timeBetweenLowerings);
        Roof.transform.position -= heightToLose;
        transform.position -= heightToLose;
        foreach(Transform cell in gridPlace)
        {
            if (cell.GetComponent<GridPlace>().Bobble)
            {
                cell.GetComponent<GridPlace>().Bobble.GetComponent<BallBehaviour>().LowerMe(heightToLose);
            }            
        }

        StartCoroutine("LowerGrid");
    }

    public void ClearBallsSound()
    {
        soundSpeaker.PlayOneShot(soundClip[0]);
    }

    public void ReturnToOrigin()
    {
        Roof.transform.position = startRoof;
        transform.position = startPos;
    }

    public bool Victory()
    {
        if (gameManager.canPlay)
        {
            foreach (Transform cell in gridPlace)
            {
                if (cell.GetComponent<GridPlace>().Bobble)
                {
                    return false;
                }
            }
            return true;
        }
        return false;
    }

    public bool Lose()
    {
        foreach (Transform cell in gridPlace)
        {
            if (cell.GetComponent<GridPlace>().Bobble && cell.GetComponent<GridPlace>().Bobble.GetComponent<BallBehaviour>().LosingPosition())
            {
                return true;
            }
        }
        return false;
    }
}