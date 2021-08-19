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
        RaycastHit hit;
        List<Transform> surrounding = new List<Transform>();
        
        foreach (Ray ray in rays) 
        {
            if (Physics.Raycast(ray, out hit, rayLength)) 
            {
                if (hit.transform.gameObject.tag == "Bobble") {
                    string hitColor = hit.transform.gameObject.GetComponent<raycasting>().ballColor;
                    if (ballColor == hitColor || colorCheck == false) {
                        Vector3 hitPos = hit.transform.position - transform.position;  

                        // Visual debug
                        Debug.DrawRay(transform.position, hitPos, Color.green);

                        if (!blacklist.Contains(hit.transform)) {
                            surrounding.Add(hit.transform);
                        }
                    }
                }

            }
        }
        return surrounding;
    }


    public void drawRays()
    {
        // Define all 6 direction that need to be checked
        rays[0] = new Ray(transform.position, Vector3.left);
        rays[1] = new Ray(transform.position, (Vector3.left + Vector3.up).normalized);
        rays[2] = new Ray(transform.position, (Vector3.right + Vector3.up).normalized);
        rays[3] = new Ray(transform.position, Vector3.right);
        rays[4] = new Ray(transform.position, (Vector3.left + Vector3.down).normalized);
        rays[5] = new Ray(transform.position, (Vector3.right + Vector3.down).normalized);
    }
}
