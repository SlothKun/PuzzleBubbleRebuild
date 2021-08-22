using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GridScript gridLogic;

    void Start()
    {
        StartCoroutine("StartingGame");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator StartingGame()
    {
        yield return new WaitForSeconds(3f);
        gridLogic.StartCoroutine("LowerGrid");
    }

    public void EndRound()
    {
        gridLogic.StopAllCoroutines();
    }

    private IEnumerator NewRound()
    {
        yield return new WaitForSeconds(2f);
        gridLogic.ReturnToOrigin();
    }
}
