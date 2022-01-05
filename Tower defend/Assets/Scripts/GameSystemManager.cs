using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSystemManager : MonoBehaviour
{
    public static GameSystemManager Instance { get; private set; }
    public MoneySystem moneySystem;
    public BuildSystem buildSystem;
    public DamageSystem damageSystem;
    public GUISystem guiSystem;
    private void Awake()
    {
        Instance = this;
    }
}
