using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DI_Ssytem : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private DamageIndicator indicatorPrefab = null;
    [SerializeField] private RectTransform holder = null;
    [SerializeField] private new Camera camera = null;
    [SerializeField] private Transform player = null;
    private Dictionary<Transform, DamageIndicator> Indicator = new Dictionary<Transform, DamageIndicator>();
    #region Delegates
    public static Action<Transform> CreateIndicator = delegate { };
    #endregion
    private void OnEnable() //subcrisbe funcion
    {
        Debug.Log("Enabled");
        CreateIndicator += Create;
    }
    private void OnDisable() //unsubcrisbe funcion
    {
        Debug.Log("Disabled");
        CreateIndicator -= Create;
    }
    void Create(Transform target)
    {
        if (Indicator.ContainsKey(target))
        {
            Indicator[target].Restart();
            return;
        }
        DamageIndicator newIndicator = Instantiate(indicatorPrefab, holder);
        newIndicator.Register(target, player, new Action(() => { Indicator.Remove(target); }));
        Indicator.Add(target, newIndicator);
    }
}
