using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControls : MonoBehaviour
{
    [Header ("Stats and checks")]

    public float rotateSpeed;
    [SerializeField] private float minRotation;
    [SerializeField] private float maxRotation;

    private int rotationMagnitude;
    private float z;

    public bool isLoaded;

    [Header ("Other objects")]

    public GameObject loadedBall;

    [SerializeField] private BallSpawn ballSpawn;

    public GameManager gameManager;

    public Vector3 directionToShoot;

    private void Start()
    {
        z = 0f;
        rotationMagnitude = 0;
    }

    private void Update()
    {
        directionToShoot = transform.up;
    }

    public void OnTurningLeft(InputAction.CallbackContext button)
    {
        if (button.performed)
        {
            rotationMagnitude = 1;
        }
        else if (button.canceled)
        {
            rotationMagnitude = 0;
        }
    }

    public void OnTurningRight(InputAction.CallbackContext button)
    {
        if (button.performed)
        {
            rotationMagnitude = -1;
        }
        else if (button.canceled)
        {
            rotationMagnitude = 0;
        }
    }

    private void Rotate()
    {
        if (rotationMagnitude != 0 && gameManager.canPlay)
        {
            z += Time.fixedDeltaTime * rotationMagnitude * rotateSpeed;

            if (z >= maxRotation)
            {
                z = maxRotation;
            }
            if (z <= minRotation)
            {
                z = minRotation;
            }

            transform.localRotation = Quaternion.Euler(0, 0, z);
        }
    }

    public void OnFire(InputAction.CallbackContext button)
    {
        if (button.performed && isLoaded && gameManager.canPlay)
        {
            loadedBall.GetComponent<BallBehaviour>().OnShooting(directionToShoot);
            isLoaded = false;
            loadedBall.GetComponent<ballcontroller>().lastShot = true;
            loadedBall = null;
            ballSpawn.TransferToCannon();
            ballSpawn.SpawnNewBall();
        }
    }

    private void FixedUpdate()
    {
        Rotate();
    }
}
