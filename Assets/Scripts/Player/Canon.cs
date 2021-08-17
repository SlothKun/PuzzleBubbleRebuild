using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Canon : MonoBehaviour
{
    public int nbrPoints;

    public GameObject pointsPref;
    public GameObject[] Points;

    public Vector2 currentPointPos;

    private float collisionCheckRadius = 1f;

    public void Start()
    {
        Points = new GameObject[nbrPoints];

        for (int i = 0; i < nbrPoints; i++)
        {
            Points[i] = Instantiate(pointsPref, transform.position, transform.rotation); //On fait spawn un certain nombre de point
        }
    }

    public void Update()
    {
        for (int i = 0; i < Points.Length; i++)
        {
            Points[i].transform.position = CalculatePosition(1f * i); //On place les points

            /*if (CheckForCollision(currentPointPos)) //Si on detecte une collision on arrete de spawn les points
            {
                Debug.Log("ALED");
                break;
            }*/
        }
    }

    public Vector2 CalculatePosition(float time)
    {
        Vector2 currentPointPos = (Vector2)transform.position + (Vector2)transform.up * time; //Gere la separation entre les points
        return currentPointPos;
    }

    /*public bool CheckForCollision(Vector2 position)
    {
        Collider[] hits = Physics.OverlapSphere(position, collisionCheckRadius);

        if (hits.Length > 0)
        {
            Debug.Log("HITORMISSIGUESSITSNEVERMISS");
            for (int x = 0; x < hits.Length; x++)
            {
                if (hits[x].tag != "Player" && hits[x].tag != "Floor")
                {
                    Debug.Log("VRAIIIIIIIIIIIII");
                    return true;
                }
            }

        }
        return false;
    }*/
}
