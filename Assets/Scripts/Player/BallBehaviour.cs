using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallBehaviour : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    public bool isMoving;
    private Vector3 Direction;

    void Update()
    {
        if (isMoving)
        {
            transform.position += Direction * moveSpeed * Time.deltaTime;
        }
    }

    public void OnShooting(Vector3 canonDirection)
    {
        isMoving = true;
        Direction = canonDirection;
    }
}
