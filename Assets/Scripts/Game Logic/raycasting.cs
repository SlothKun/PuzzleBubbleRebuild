using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class raycasting : MonoBehaviour
{
    [SerializeField] private float rayLength;
    [SerializeField] private string ballColor;
    public string colorCheck;
    private Ray[] rays = new Ray[6];

    void Start()
    {
        ballColor = GetComponent<BallIdentity>().myColor;
    }

    // Added
    public List<Transform> GetSurrounding(HashSet<Transform> blacklist, bool colorCheck) 
    {
        drawRays();
        RaycastHit hit;
        List<Transform> surrounding = new List<Transform>();
        
        foreach (Ray ray in rays) 
        {
            if (Physics.Raycast(ray, out hit, rayLength)) 
            {
                if (hit.transform.gameObject.tag.Equals("Bobble")) {
                    string hitColor = hit.transform.gameObject.GetComponent<raycasting>().ballColor;
                    if (ballColor == hitColor || colorCheck == false) {
                        Vector3 hitPos = hit.transform.position - transform.position;  

                        if (!blacklist.Contains(hit.transform)) {
                            // Visual debug
                            //Debug.DrawRay(transform.position, hitPos, Color.green);
                            surrounding.Add(hit.transform);
                        }
                    }
                }
            }
        }
        return surrounding;
    }


    public List<Transform> FindSet(HashSet<Transform> comboBalls) {
        bool newadd = true;
        List<Transform> tempDetected = new List<Transform>();
        List<Transform> set = new List<Transform>();
        HashSet<Transform> blacklist = new HashSet<Transform>();
        Transform currentBall = transform;

        set.Add(currentBall);
        blacklist.Add(currentBall);
        blacklist.UnionWith(comboBalls);
        while (newadd) {
            tempDetected.Clear();
            newadd = false;
            foreach (Transform ball in set) {
                tempDetected.AddRange(ball.gameObject.GetComponent<raycasting>().GetSurrounding(blacklist, false));
                blacklist.UnionWith(tempDetected);
                if (tempDetected.Count != 0) newadd = true;
            }
            set.AddRange(tempDetected);
        }
        print("Set : " + set.Count);
        return set;
    }
    public bool IsAttachedToRoof() {
        RaycastHit hit;
        drawRays();

        bool attached = false;
        for (int i=1; i < 3; i++) {
            if (Physics.Raycast(rays[i], out hit, rayLength)) {
                Vector3 hitPos = hit.transform.position - transform.position;
                // Visual debug
                //Debug.DrawRay(transform.position, hitPos, Color.green);
                if (hit.transform.gameObject.tag.Equals("Roof")) {
                    attached = true;
                }
            }
        }
        return attached;
    }

    public void drawRays()
    {
        // Define all 6 direction that need to be checked
        rays[0] = new Ray(transform.position, Vector3.left); // Left
        rays[1] = new Ray(transform.position, (Vector3.left + Vector3.up).normalized); // Top Left
        rays[2] = new Ray(transform.position, (Vector3.right + Vector3.up).normalized); // Top right
        rays[3] = new Ray(transform.position, Vector3.right); // Right
        rays[4] = new Ray(transform.position, (Vector3.right + Vector3.down).normalized); // Down Right
        rays[5] = new Ray(transform.position, (Vector3.left + Vector3.down).normalized); // Down left
    }
}
