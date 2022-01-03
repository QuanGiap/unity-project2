using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderManager : MonoBehaviour
{
    public Slider Health;
    public Slider Shield;
    public void SetHealthAndShield(float MaxHealth, float MaxShield)
    {
        Health.maxValue = MaxHealth;
        Shield.maxValue = MaxShield;
    }
    public void ChangeHealthValue(float HealthValue)
    {
        Health.value = HealthValue;
    }
    public void ChangeShieldValue(float ShieldValue)
    {
        Shield.value = ShieldValue;
    }
}
