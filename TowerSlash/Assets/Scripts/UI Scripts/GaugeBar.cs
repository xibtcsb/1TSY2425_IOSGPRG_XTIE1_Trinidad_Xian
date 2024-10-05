using UnityEngine;
using UnityEngine.UI;

public class GaugeBar : MonoBehaviour
{
    [SerializeField] private Image gaugeBar;
    [SerializeField] private float maxGaugeAmount = 100f;
    [SerializeField] private float gaugeAmount = 0f;

    private void Start()
    {
        gaugeAmount = 0f;
        UpdateGaugeUI();
    }

    public void EnemyKilled(bool isSpeedCharacter)
    {
        float amountToAdd = isSpeedCharacter ? maxGaugeAmount * 0.1f : maxGaugeAmount * 0.05f;
        gaugeAmount += amountToAdd;
        gaugeAmount = Mathf.Clamp(gaugeAmount, 0, maxGaugeAmount);
        Debug.Log($"Enemy killed! Gauge Amount: {gaugeAmount}");
        UpdateGaugeUI();
    }

    public bool CanDash()
    {
        return gaugeAmount == maxGaugeAmount;
    }

    public void UseDash()
    {
        if (CanDash())
        {
            Debug.Log("Dash used!");
            gaugeAmount = 0f;
            UpdateGaugeUI();
        }
        else
        {
            Debug.Log("Not enough gauge to dash.");
        }
    }

    private void UpdateGaugeUI()
    {
        gaugeBar.fillAmount = gaugeAmount / maxGaugeAmount;
        Debug.Log($"Gauge UI updated: {gaugeBar.fillAmount}");
    }

}
