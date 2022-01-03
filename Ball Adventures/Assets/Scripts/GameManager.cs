using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public Button[] lvlButton;
    // Start is called before the first frame update
    void Start()
    {
        int levelAt = PlayerPrefs.GetInt("levelAt", 1);
        for (int i = 0; i < lvlButton.Length; i++)
        { if (i + 1 > levelAt) lvlButton[i].interactable = false; }
    }
}
