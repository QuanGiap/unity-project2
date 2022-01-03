using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BossHealthBar : MonoBehaviour
{
    public Slider Slider;
    public Gradient gradient;
    public Image fill;
    public void SetMaxHealth(int Health)
    { Slider.maxValue = Health;
        Slider.value = Health;
        fill.color = gradient.Evaluate(1f);
    }
    public void SetHealth(int Health)
    {
        Slider.value = Health;
        fill.color = gradient.Evaluate(Slider.normalizedValue);
    }

}
