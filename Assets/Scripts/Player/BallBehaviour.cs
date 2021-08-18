using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallBehaviour : MonoBehaviour
{
    [Header("In Game")]
    public Transform closestPlace;
    private List<GameObject> otherBalls = new List<GameObject>();
    [SerializeField] GridScript gridScript;    

    [SerializeField] private float moveSpeed;
    public bool isMoving;
    public bool Shot;
    public bool Placed;
    private Vector3 Direction;
    

    private void Start()
    {
        gridScript = GameObject.Find("Grid").GetComponent<GridScript>();
        if (GetComponent<BallIdentity>().starterBobble)
        {
            PlaceMe();
        }
    }

    void FixedUpdate()
    {
        if (isMoving && !Placed)
        {
            transform.position += Direction * moveSpeed * Time.fixedDeltaTime;
            DetectCollision();
        }
    }

    public void OnShooting(Vector3 canonDirection)
    {
        isMoving = true;
        Shot = true;
        Direction = canonDirection;
        otherBalls.AddRange(GameObject.FindGameObjectsWithTag("Bobble"));
    }

    private void DetectCollision()
    {
        foreach(GameObject bobble in otherBalls)
        {
            if (!bobble.Equals(this.gameObject))
            {
                if (Vector3.Distance(transform.position, bobble.transform.position) <= 0.6f)
                {
                    PlaceMe();
                }
            }
        }
    }

    private void PlaceMe()
    {
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
        Placed = true;
        closestPlace.gameObject.GetComponent<GridPlace>().occupied = true;
        closestPlace.gameObject.GetComponent<GridPlace>().Bobble = this.gameObject;
        isMoving = false;
        GetComponent<raycasting>().drawRays();
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
