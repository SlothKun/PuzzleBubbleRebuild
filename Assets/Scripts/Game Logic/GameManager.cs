using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GridScript gridLogic;
    public int score;
    public Text scoreText;

    public bool canPlay;

    public GameObject ballSpawner;
    public GameObject retryScreen;

    [SerializeField] private GameObject[] Level;
    [SerializeField] private int currentLevel;
    [SerializeField] private GameObject loadedLevel;

    public Vector3 levelPosition;

    void Start()
    {
        currentLevel = 0;
        canPlay = false;
        StartCoroutine("StartingGame");
        LoadLevel();
        ballSpawner.GetComponent<BallSpawn>().StartingBall();
        ballSpawner.GetComponent<BallSpawn>().SpawnNewBall();
    }

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
        canPlay = true;
        gridLogic.StartCoroutine("LowerGrid");
    }

    public void EndRound()
    {
        gridLogic.StopAllCoroutines();
        gridLogic.ReturnToOrigin();
        canPlay = false;
        Debug.Log("Win");
        StartCoroutine("NewRound");
    }

    private IEnumerator NewRound()
    {
        yield return new WaitForSeconds(2f);
        currentLevel++;
        LoadLevel();
    }

    public void LoseRound()
    {
        gridLogic.StopAllCoroutines();
        canPlay = false;
        Debug.Log("Lose");
        retryScreen.SetActive(true);
    }

    public void AddScore(int scoretoadd)
    {
        score += scoretoadd;
        scoreText.text = ("" + score);
    }

    public void LoadLevel()
    {
        if (loadedLevel)
        {
            Destroy(loadedLevel.gameObject);
        }

        loadedLevel = Instantiate(Level[currentLevel], levelPosition, Quaternion.identity);
        StartCoroutine("StartingGame");
    }

    public void ReloadGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
