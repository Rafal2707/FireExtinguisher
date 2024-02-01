using UnityEngine;
using UnityEngine.UI;
public class PowderBarUI : MonoBehaviour
{
    [SerializeField] private Slider powderBar;
    [SerializeField] private FireExtinguisher fireExtinguisher;

    float maxValue = 100f;
    float minValue = 0f;

    private void Update()
    {
        if (minValue < fireExtinguisher.GetPowderLeft() && fireExtinguisher.GetPowderLeft() < maxValue)
        {
            UpdateFireStrengthBar();
        }
    }
    private void UpdateFireStrengthBar()
    {
        powderBar.value = fireExtinguisher.GetPowderLeft();
    }
}
