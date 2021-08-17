using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallIdentity : MonoBehaviour
{
    [SerializeField] private string[] possibleColors;
    public string myColor;
    [SerializeField] private Renderer thisRenderer;

    private void Awake()
    {
        thisRenderer = GetComponent<Renderer>();
    }

    void Start()
    {
        AssignColor();
        DisplayColor();
    }

    void AssignColor()
    {
        myColor = possibleColors[Random.Range(0, possibleColors.Length)];
    }

    void DisplayColor()
    {
        switch (myColor)
        {
            case "Red":
                thisRenderer.material.color = Color.red;
                break;
            case "Yellow":
                thisRenderer.material.color = Color.yellow;
                break;
            case "Green":
                thisRenderer.material.color = Color.green;
                break;
            case "Blue":
                thisRenderer.material.color = Color.blue;
                break;
            case "Black":
                thisRenderer.material.color = Color.black;
                break;
            case "Purple":
                thisRenderer.material.color = Color.magenta;
                break;
        }        
    }
}
