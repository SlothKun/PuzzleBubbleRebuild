using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ballcontroller : MonoBehaviour
{
    // Use of hashset to not bother with duplicates
    private List<Transform> ballhit = new List<Transform>();
    private List<Transform> newhit = new List<Transform>();
    private List<Transform> neighbours = new List<Transform>(); // added
    private List<Transform> ballsToBeChecked = new List<Transform>(); // added
    private List<Transform> tempNeighboursDetected = new List<Transform>(); // added

    private HashSet<Transform> blacklist = new HashSet<Transform>(); // added
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
                    if (newhit.Count != 0) newadd = true;
                }
                ballhit.AddRange(newhit);
                //print(ballhit.Count);
            }

            if (ballhit.Count >= minCombo)
            {
                // Check destroyed balls' neighbours and put them in a list
                newadd = true;
                print("ball hit : " + ballhit.Count);
                ballsToBeChecked.AddRange(ballhit);
                blacklist.UnionWith(ballhit); // These ball will be destroy so they need not to be checked
                while (newadd)
                {
                    newadd = false;
                    blacklist.UnionWith(neighbours); // Add already checked balls to avoid duplicates
                    foreach (Transform ball in ballsToBeChecked) {
                        tempNeighboursDetected.AddRange(ball.gameObject.GetComponent<raycasting>().GetSurrounding(blacklist));
                        blacklist.UnionWith(tempNeighboursDetected); // Add new detected to the blacklist to avoid duplicates
                        if (tempNeighboursDetected.Count != 0) {
                            newadd = true;
                        }
                    }
                    neighbours.AddRange(tempNeighboursDetected);
                    ballsToBeChecked.Clear();
                    ballsToBeChecked.AddRange(tempNeighboursDetected);
                    tempNeighboursDetected.Clear();
                }
                blacklist.Clear();
                DestroyChainedBalls(ballhit);

                print("neighbours : " + neighbours.Count);

                // check each ball 1 by 1
                // If checked ball of its surrounding doesn't have surrounding pos 1&2 fixed
                // Then contaminate the whole chain
                // But, if 1 ball in the chain is fixed to the ceiling -> decontaminate
                // Make sure neighbours is cleared
                // end
                
            }

            ballhit.Clear();

            lastShot = false;
        }
    }

    private void DestroyChainedBalls(List<Transform> ballhit) 
    {
        foreach (Transform ball in ballhit) 
        {
            ball.gameObject.GetComponent<BallBehaviour>().otherBalls.Remove(ball.gameObject);
            Destroy(ball.gameObject);
        }
    }
}
