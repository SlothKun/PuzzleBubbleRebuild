using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallIdentity : MonoBehaviour
{
    [SerializeField] private string[] possibleColors;
    public string myColor;
    public string startingColor;
    private Animator thisRenderer;

    public bool starterBobble;

    private void Awake()
    {
        AssignColor();
        thisRenderer = GetComponent<Animator>();
    }

    void Start()
    {
        DisplayColor();
    }

    void AssignColor()
    {
        if (!starterBobble)
        {
            myColor = possibleColors[Random.Range(0, possibleColors.Length)];
        }
        else
        {
            myColor = startingColor;
        }
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
