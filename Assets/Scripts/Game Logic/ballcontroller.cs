using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ballcontroller : MonoBehaviour
{
    // Use of hashset to not bother with duplicates
    private HashSet<Transform> ballsToBeChecked = new HashSet<Transform>();
    private List<Transform> newhit = new List<Transform>();
    private List<Transform> neighbours = new List<Transform>(); // added
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
        ballsToBeChecked.Add(transform);
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
                foreach (Transform ball in ballsToBeChecked)
                {
                    newhit.AddRange(ball.gameObject.GetComponent<raycasting>().GetSurrounding(ballsToBeChecked, true));
                    if (newhit.Count != 0) newadd = true;
                }
                ballsToBeChecked.UnionWith(newhit);
            }

            if (ballsToBeChecked.Count >= minCombo)
            {
                print("ball hit : " + ballsToBeChecked.Count);
                blacklist.UnionWith(ballsToBeChecked); // These ball will be destroy so they need not to be checked
                // Check wannabe destroyed balls' neighbours and put them in a list
                foreach (Transform ball in ballsToBeChecked) {
                    tempNeighboursDetected.AddRange(ball.gameObject.GetComponent<raycasting>().GetSurrounding(blacklist, false));
                    blacklist.UnionWith(tempNeighboursDetected); // Add new detected to the blacklist to avoid duplicates
                    // Find set of neighbours
                    // Put them in neighbours set (a list of list)
                    neighbours.AddRange(tempNeighboursDetected); // Get rid of this after
                    
                    tempNeighboursDetected.Clear();
                }

                DestroyChainedBalls(ballsToBeChecked);
                blacklist.Clear();

                print("neighbours : " + neighbours.Count);

                // check each ball 1 by 1
                // If checked ball of its surrounding doesn't have surrounding pos 1&2 fixed
                // Then contaminate the whole chain
                // But, if 1 ball in the chain is fixed to the ceiling -> decontaminate
                // Make sure neighbours is cleared
                // end
                
            }

            ballsToBeChecked.Clear();

            lastShot = false;
        }
    }

    private void DestroyChainedBalls(HashSet<Transform> ballhit) 
    {
        foreach (Transform ball in ballhit) 
        {
            ball.gameObject.GetComponent<BallBehaviour>().otherBalls.Remove(ball.gameObject);
            Destroy(ball.gameObject);
        }
    }
}
