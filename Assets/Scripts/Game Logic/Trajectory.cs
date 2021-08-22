using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trajectory : MonoBehaviour
{
    public float LineOffset = 0.01f;

    private Vector3 direction;

    [SerializeField] private GameObject canonTip;
    private Vector3 startPosition;

    [SerializeField] private PlayerControls canon;
    [SerializeField] private LineRenderer lineRenderer;
    
    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        canon = GetComponent<PlayerControls>();

        lineRenderer.positionCount = 3;
    }

    private void Update()
    {
        startPosition = canonTip.transform.position;
        lineRenderer.SetPosition(0, startPosition);
        direction = canon.directionToShoot;

        ReflectOnCollision();
    }

    private void ReflectOnCollision()
    {
        Vector3 origin = startPosition + LineOffset * direction;
        RaycastHit hit;
        int layerMask = 1 << 7 | 1 << 6;

        if (Physics.Raycast(origin, direction, out hit, Mathf.Infinity, layerMask))
        {
            if (hit.transform.gameObject.tag.Equals("Walls"))
            {
                lineRenderer.SetPosition(1, hit.point);

                direction = Vector3.Reflect(direction.normalized, hit.normal);
                origin = hit.point + LineOffset * direction;                

                RaycastHit secondHit;
                if(Physics.Raycast(origin, direction, out secondHit, Mathf.Infinity, layerMask))
                {
                    if (secondHit.transform.gameObject.tag.Equals("Bobble") && secondHit.transform.gameObject.GetComponent<BallBehaviour>().Placed)
                    {
                        lineRenderer.SetPosition(2, secondHit.point);
                    }
                    else if (secondHit.transform.gameObject.tag.Equals("Roof"))
                    {
                        lineRenderer.SetPosition(2, secondHit.point);
                    }
                }
            }
            else if(hit.transform.gameObject.tag.Equals("Bobble") && hit.transform.gameObject.GetComponent<BallBehaviour>().Placed)
            {
                lineRenderer.SetPosition(1, hit.point);
                lineRenderer.SetPosition(2, hit.point);
            }
            else if (hit.transform.gameObject.tag.Equals("Roof"))
            {
                lineRenderer.SetPosition(1, hit.point);
                lineRenderer.SetPosition(2, hit.point);
            }
        }

        lineRenderer.material.SetTextureScale("_MainTex", new Vector2 (0.2f, 0.2f));
    }
}
