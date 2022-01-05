using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class SystemManager : MonoBehaviour
{
    public TextMeshProUGUI TextScore;
    public bool IsGameStart = false;
    // Start is called before the first frame update
    void Start()
    {
        TextScore.text = ("Score: 0");
    }

    public void GameStart()
    {
        IsGameStart = true;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public void UpdateScore(int score)
    {
        TextScore.text = ("Score: " + score);
    }
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
