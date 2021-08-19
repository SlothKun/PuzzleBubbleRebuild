using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallIdentity : MonoBehaviour
{
    [SerializeField] private string[] possibleColors;
    [SerializeField] private List<string> currentColors = new List<string>();
    [SerializeField] private List<GameObject> otherBalls = new List<GameObject>();
    public string myColor;
    public string startingColor;
    public bool falling; // Added
    private Animator thisRenderer;

    public bool starterBobble;

    private void Awake()
    {
        falling = false; // Added
        AssignColor();
        thisRenderer = GetComponent<Animator>();
    }

    void Start()
    {
        DisplayColor();
    }

    private void CheckOtherBalls()
    {
        otherBalls.AddRange(GameObject.FindGameObjectsWithTag("Bobble"));

        foreach (GameObject ball in otherBalls)
        {
            string newColor = ball.GetComponent<BallIdentity>().myColor;
            if (!currentColors.Contains(newColor))
            {
                currentColors.Add(newColor);
            }
        }
    }

    void AssignColor()
    {
        if (!starterBobble)
        {
            CheckOtherBalls();
            myColor = currentColors[Random.Range(0, currentColors.Count - 1)];
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
