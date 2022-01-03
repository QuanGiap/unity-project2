using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishGame : MonoBehaviour
{
    public GameObject TextBox;
    public int NextSceneLoad;
    private void Start()
    {
        NextSceneLoad = SceneManager.GetActiveScene().buildIndex + 1;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            TextBox.gameObject.SetActive(true);
        }
        if (NextSceneLoad > PlayerPrefs.GetInt("levelAt"))
        { PlayerPrefs.SetInt("levelAt", NextSceneLoad); }
    }
}
