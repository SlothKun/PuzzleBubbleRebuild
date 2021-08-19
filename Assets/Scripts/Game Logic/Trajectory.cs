using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trajectory : MonoBehaviour
{
    public int nbrPoints;
    public int TotalBounce = 3;
    public float LineOffset = 0.01f;

    public GameObject pointsPref;
    public GameObject[] Points;

    RaycastHit hit;

    public Vector3 currentPointPos;
    public Vector3 direction;

    public PlayerControls cannon;
    
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
        direction = cannon.directionToShoot;

        for (int i = 0; i < Points.Length; i++)
        {
            Points[i].transform.position = CalculatePosition(1f * i); //On place les points
            ReflectOnCollision(Points[i].transform.position); //Reflect en contact d'un mur
        }
    }

    public Vector3 CalculatePosition(float time)
    {
        Vector3 currentPointPos = transform.position + transform.up * time; //Gere la separation entre les points
        return currentPointPos;
    }

    public void ReflectOnCollision(Vector3 position)
    {
        Vector3 origin = cannon.gameObject.transform.position + LineOffset * direction;

        for (int x = 1; x < TotalBounce; x++)
        {
            if (Physics.Raycast(origin, direction, out hit))
            {
                direction = Vector3.Reflect(direction.normalized, hit.normal);
                origin = hit.point + LineOffset * direction;
            }
        }
    }
}
