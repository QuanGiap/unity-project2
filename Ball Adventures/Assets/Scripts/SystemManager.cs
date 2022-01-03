using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SystemManager : MonoBehaviour
{
    public void ExitGame()
    {
        PlayerPrefs.DeleteAll();
        Application.Quit();
    }
    public void ScreenLv1()
    {
        SceneManager.LoadScene("Lv 1");
    }
    public void ScreenLv2()
    {
        SceneManager.LoadScene("Lv 2");
    }
    public void ScreenLv3()
    {
        SceneManager.LoadScene("Lv 3");
    }
    public void ScreenLv4()
    {
        SceneManager.LoadScene("Lv 4");
    }
    public void ScreenLv5()
    {
        SceneManager.LoadScene("Lv 5");
    }
    public void ScreenLv6()
    {
        SceneManager.LoadScene("Lv 6");
    }
    public void RetrunStartScreen()
    {
        SceneManager.LoadScene("Start Screen");
    }
}
