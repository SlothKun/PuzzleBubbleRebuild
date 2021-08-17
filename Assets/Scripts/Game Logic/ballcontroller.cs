using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ballcontroller : MonoBehaviour
{
    // Use of hashset to not bother with duplicates
    public List<Transform> ballhit = new List<Transform>();
    public List<Transform> newhit = new List<Transform>();
    private bool newadd;
    public bool lastShot;
    [SerializeField] private BallBehaviour ballBehaviour;

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

            if (ballhit.Count >= 3)
            {
                DestroyChainedBalls(ballhit);
            }

            ballhit.Clear();

            lastShot = false;
        }
    }

    void DestroyChainedBalls(List<Transform> ballhit) {
        foreach (Transform ball in ballhit) {
            Destroy(ball.gameObject);
        }
    }
}
