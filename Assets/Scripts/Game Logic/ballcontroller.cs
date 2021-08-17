using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ballcontroller : MonoBehaviour
{
    // Use of hashset to not bother with duplicates
    private List<Transform> ballhit = new List<Transform>();
    private List<Transform> newhit = new List<Transform>();
    private bool newadd;
    public bool lastShot;
    [SerializeField] private BallBehaviour ballBehaviour;
    [SerializeField] private int minCombo;

    private void Awake()
    {
        ballBehaviour = GetComponent<BallBehaviour>();
    }

    // Start is called before the first frame update
    void Start()
    {
        ballhit.Add(transform);
        newadd = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (lastShot && ballBehaviour.Placed)
        {
            while (newadd)
            {
                newhit.Clear();
                newadd = false;
                foreach (Transform ball in ballhit)
                {
                    newhit.AddRange(ball.gameObject.GetComponent<raycasting>().Raycast(ballhit));
                    if (newhit.Count != 0)
                    {
                        newadd = true;
                    }
                }
                ballhit.AddRange(newhit);
                print(ballhit.Count);
            }

            if (ballhit.Count >= minCombo)
            {
                DestroyChainedBalls(ballhit);
            }

            ballhit.Clear();

            lastShot = false;
        }
    }

    private void DestroyChainedBalls(List<Transform> ballhit) 
    {
        foreach (Transform ball in ballhit) 
        {
            ballBehaviour.closestPlace.gameObject.GetComponent<GridPlace>().occupied = false;
            Destroy(ball.gameObject);
        }
    }
}
