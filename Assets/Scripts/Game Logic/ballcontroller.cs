using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ballcontroller : MonoBehaviour
{
    // Use of hashset to not bother with duplicates
    private HashSet<Transform> ballsToBeChecked = new HashSet<Transform>();
    private List<Transform> newhit = new List<Transform>();
    private List<List<Transform>> neighboursSet = new List<List<Transform>>(); // added
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
            // Get balls combo
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
                
                // Check set of ball around comboBall
                foreach (Transform ball in ballsToBeChecked) 
                {
                    bool alreadyInSet = false;
                    tempNeighboursDetected.AddRange(ball.gameObject.GetComponent<raycasting>().GetSurrounding(blacklist, false));
                    blacklist.UnionWith(tempNeighboursDetected); // Add new detected to the blacklist to avoid duplicates

                    foreach (Transform tempNeighbour in tempNeighboursDetected) 
                    {
                        // If ball is already in a set, there's no need to check it
                        foreach (List<Transform> set in neighboursSet) 
                        {
                            if (set.Contains(tempNeighbour)) 
                            {
                                alreadyInSet = true;
                            }
                        }

                        if (alreadyInSet == false) 
                        {
                            neighboursSet.Add(tempNeighbour.gameObject.GetComponent<raycasting>().FindSet(ballsToBeChecked));
                        }
                    }
                    
                    tempNeighboursDetected.Clear();
                }

                DestroyChainedBalls(ballsToBeChecked);
                blacklist.Clear();
                print("neighbours nb of set : " + neighboursSet.Count);

                // Check falling state
                foreach (List<Transform> set in neighboursSet) 
                {
                    bool falling = false;
                    bool roofhit = false;
                    string state;
                    foreach (Transform ball in set) 
                    {
                        if (!roofhit) 
                        {
                            state = ball.gameObject.GetComponent<raycasting>().IsBallFalling();
                            if (state == "falling") 
                            {
                                falling = true;
                            } 
                            else if (state == "roof") 
                            {
                                roofhit = true;
                            }
                        }
                    }
                    
                    // If ball is falling, change its state
                    if (roofhit == false && falling == true) 
                    {
                        foreach (Transform ball in set) 
                        {
                            ball.gameObject.GetComponent<BallIdentity>().falling = true;
                            ball.gameObject.GetComponent<BallBehaviour>().Fall(ball.gameObject);
                        }
                    }
                }  
            }

            neighboursSet.Clear();
            ballsToBeChecked.Clear();

            lastShot = false;
        }
    }

    private void DestroyChainedBalls(HashSet<Transform> ballhit) 
    {
        foreach (Transform ball in ballhit) 
        {
            ball.GetComponent<BallBehaviour>().DestroyBall(ball.gameObject);
        }
    }
}
