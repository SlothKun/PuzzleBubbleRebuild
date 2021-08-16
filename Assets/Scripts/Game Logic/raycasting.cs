using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class raycasting : MonoBehaviour
{
    public float rayLength;
    public string ballColor;
    public string colorCheck;
    private Ray[] rays = new Ray[6];


    void Start() 
    {   
        // Define all 6 direction that need to be checked
        rays[0] = new Ray(transform.position, Vector3.left);
        rays[1] = new Ray(transform.position, (Vector3.left + Vector3.up).normalized);
        rays[2] = new Ray(transform.position, (Vector3.right + Vector3.up).normalized);
        rays[3] = new Ray(transform.position, Vector3.right);
        rays[4] = new Ray(transform.position, (Vector3.left + Vector3.down).normalized);
        rays[5] = new Ray(transform.position, (Vector3.right + Vector3.down).normalized);

        rayLength = 50; // A definir quand on aura la taille finale des boules
    }

    // Update is called once per frame
    void Update()
    {  
        colorCheck = ballColor; // Put that in start after testing
        RaycastHit hit;
        
        foreach (Ray ray in rays) {
            if (Physics.Raycast(ray, out hit, rayLength)) {
                string hitColor = hit.transform.gameObject.GetComponent<raycasting>().ballColor;
                if (ballColor == hitColor) {
                    Vector3 hitPos = hit.transform.position - transform.position;
                    Debug.DrawRay(transform.position, hitPos, Color.green);
                }
            }
        }
    }
}
