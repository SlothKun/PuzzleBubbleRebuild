using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallIdentity : MonoBehaviour
{
    [SerializeField] private string[] possibleColors;
    public string myColor;
    private Animator thisRenderer;

    private void Awake()
    {
        thisRenderer = GetComponent<Animator>();
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
                thisRenderer.Play("RedBobble");
                break;
            case "Yellow":
                thisRenderer.Play("YellowBobble");
                break;
            case "Green":
                thisRenderer.Play("GreenBobble");
                break;
            case "Blue":
                thisRenderer.Play("BlueBobble");
                break;
            case "Black":
                thisRenderer.Play("BlackBobble");
                break;
            case "Purple":
                thisRenderer.Play("PurpleBobble");
                break;
        }
    }
}