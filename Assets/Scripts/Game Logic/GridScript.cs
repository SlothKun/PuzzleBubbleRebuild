using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridScript : MonoBehaviour
{
    private Vector3 startPos;
    private Vector3 heightToLose = new Vector3 (0f, 0.52f, 0f);

    public Transform[] gridPlace;
    public GameObject Roof;

    public float timeBetweenLowerings;

    private void Start()
    {
        startPos = transform.position;
    }
    void FixedUpdate()
    {
        if (Victory())
        {
            Debug.Log("Victory");
            //DO SOMETHING
        }
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
}
