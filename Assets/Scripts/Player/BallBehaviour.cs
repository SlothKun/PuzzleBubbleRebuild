using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallBehaviour : MonoBehaviour
{
    [Header("Grid tracking")]
    public Transform closestPlace;
    public List<GameObject> otherBalls = new List<GameObject>();
    [SerializeField] private GridScript gridScript;    

    [Header ("Stats")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float fallSpeed;
    [SerializeField] private float reflectOffset;
    [SerializeField] private int scoreToAdd;

    [Header ("Checks")]
    public bool isMoving;
    public bool Shot;
    public bool Placed;
    public bool Destroyed;

    private Vector3 Direction;
    private Vector3 Origin;

    private void Awake()
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
            DetectWall();
            DetectCollision();
        }

        if (GetComponent<BallIdentity>().falling)
        {
            transform.position += -Vector3.up * Time.fixedDeltaTime * fallSpeed;
        }

        LosingPosition();
    }

    public void OnShooting(Vector3 canonDirection)
    {
        Origin = (Vector3)transform.position + reflectOffset * canonDirection;
        isMoving = true;
        Shot = true;
        Direction = canonDirection;
        otherBalls.AddRange(GameObject.FindGameObjectsWithTag("Bobble"));
    }

    private void DetectCollision()
    {
        foreach(GameObject bobble in otherBalls)
        {
            if (!bobble.Equals(this.gameObject) && !bobble.GetComponent<BallIdentity>().falling && !Destroyed)
            {
                if (Vector3.Distance(transform.position, bobble.transform.position) <= 0.6f)
                {
                    float yPos = transform.localPosition.y;

                    if (yPos <= -1.46)
                    {
                        isMoving = false;
                        Placed = true;
                    }
                    else
                    {
                        PlaceMe();
                    }
                    
                }
            }
        }
    }

    private void DetectWall()
    {
        int layerMask = 1 << 7;

        if (Physics.CheckSphere(transform.position, 0.1f, layerMask))
        {
            RaycastHit hit;
            if (Physics.Raycast(Origin, Direction, out hit, Mathf.Infinity, layerMask))
            {
                if (hit.transform.gameObject.tag.Equals("Roof"))
                {
                    PlaceMe();
                }
                else
                {
                    Direction = Vector3.Reflect(Direction.normalized, hit.normal);
                    Origin = hit.point + reflectOffset * Direction;
                }                
            }
        }
    }

    public bool LosingPosition()
    {
        LayerMask loseMask = 1 << 8;

        if (Placed && Physics.CheckSphere(transform.position, 0.5f, loseMask) && !GetComponent<BallIdentity>().falling && !Destroyed)
        {
            return true;
        }
        else
        {
            return false;
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

        if (!GetComponent<BallIdentity>().starterBobble)
        {
            GetComponent<AudioSource>().Play();
            GetComponent<raycasting>().drawRays();
        }        
    }

    public void LowerMe(Vector3 newPos)
    {
        closestPlace.gameObject.GetComponent<GridPlace>().occupied = true;
        closestPlace.gameObject.GetComponent<GridPlace>().Bobble = this.gameObject;

        transform.position -= newPos;

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
        closestPlace.gameObject.GetComponent<GridPlace>().Bobble = this.gameObject;
    }

    public void DestroyBall(GameObject ball)
    {
        gridScript.ClearBallsSound();
        GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().AddScore(scoreToAdd);
        Destroyed = true;
        StartCoroutine("vanishTimeOut", ball);
        Color tmp = ball.GetComponent<SpriteRenderer>().color;
        tmp.a = 0f;
        ball.GetComponent<SpriteRenderer>().color = tmp;
        ball.GetComponent<BallBehaviour>().otherBalls.Remove(ball.gameObject);
        ball.GetComponentInChildren<ParticleSystem>().Play();
    }

    private IEnumerator vanishTimeOut(GameObject ball)
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(ball.gameObject);
    }

    public void Fall(GameObject ball)
    {
        ball.gameObject.GetComponent<BallBehaviour>().otherBalls.Remove(ball.gameObject);
        GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().AddScore(scoreToAdd * 3);
    }

    private void OnBecameInvisible()
    {
        otherBalls.Remove(this.gameObject);
        Destroy(gameObject);
    }
}
