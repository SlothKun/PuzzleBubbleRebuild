using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallBehaviour : MonoBehaviour
{
    [SerializeField] GridScript gridScript;
    private Transform closestPlace;

    [SerializeField] private float moveSpeed;
    public bool isMoving;
    public bool Shot;
    public bool Placed;
    private Vector3 Direction;

    private Vector3 previousPos;

    private void Start()
    {
        gridScript = GameObject.Find("Grid").GetComponent<GridScript>();
    }

    void Update()
    {
        previousPos = transform.position;

        if (isMoving)
        {
            transform.position += Direction * moveSpeed * Time.deltaTime;
            DetectCollision();
        }
        else if (!isMoving && Shot)
        {
            Placed = true;
        }        
    }

    public void OnShooting(Vector3 canonDirection)
    {
        isMoving = true;
        Shot = true;
        Direction = canonDirection;
    }

    private void DetectCollision()
    {
        int layerMask = 1 << 6 | 1 << 7;

        RaycastHit[] hits = Physics.RaycastAll(new Ray(previousPos, (transform.position - previousPos).normalized), (transform.position - previousPos).magnitude, layerMask);

        for(int i = 0; i < hits.Length; i++)
        {
            RaycastHit hit = hits[i];
            foreach (Transform placement in gridScript.gridPlace)
            {
                if (!placement.gameObject.GetComponent<GridPlace>().occupied)
                {
                    if (!closestPlace)
                    {
                        closestPlace = placement;
                    }

                    if (Vector3.Distance(placement.position, transform.position) <= Vector3.Distance(closestPlace.position, transform.position))
                    {
                        closestPlace = placement;
                    }
                }
            }

            transform.position = closestPlace.position;
            closestPlace.gameObject.GetComponent<GridPlace>().occupied = true;
            isMoving = false;
            GetComponent<raycasting>().drawRays();
        }
    }
}
