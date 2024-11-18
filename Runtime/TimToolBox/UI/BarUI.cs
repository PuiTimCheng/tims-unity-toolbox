using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class BarUI : MonoBehaviour
{
    public TMP_Text text;
    public Slider slider;

    public void SetSliderValue(float currentHealth, float maxHealth)
    {
        slider.value = currentHealth / maxHealth;
        text.text = $"{currentHealth}/{maxHealth}";
    }
}