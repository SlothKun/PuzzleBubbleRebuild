using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GridScript gridLogic;
    public int score;
    public Text scoreText;

    void Start()
    {
        StartCoroutine("StartingGame");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (gridLogic.Victory())
        {
            EndRound();
        }

        if (gridLogic.Lose())
        {
            LoseRound();
        }
    }

    private IEnumerator StartingGame()
    {
        yield return new WaitForSeconds(3f);
        gridLogic.StartCoroutine("LowerGrid");
    }

    public void EndRound()
    {
        gridLogic.StopAllCoroutines();
        Debug.Log("Win");
    }

    private IEnumerator NewRound()
    {
        yield return new WaitForSeconds(2f);
        gridLogic.ReturnToOrigin();
    }

    public void LoseRound()
    {
        Debug.Log("Lose");
    }

    public void AddScore(int scoretoadd)
    {
        score += scoretoadd;
        scoreText.text = "Score : " + score;
    }
}
