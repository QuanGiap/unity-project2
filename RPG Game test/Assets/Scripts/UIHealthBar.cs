using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIHealthBar : MonoBehaviour
{
    private Slider HealthSlide;
    [SerializeField] private RubyController rubyController;
    void Start()
    {
        HealthSlide = GetComponent<Slider>();
        HealthSlide.maxValue = rubyController.maxHealth;
        HealthSlide.value = HealthSlide.maxValue;
    }
    public void ChangeSlideValue(int val)
    {
        HealthSlide.value += val;
    }
}
