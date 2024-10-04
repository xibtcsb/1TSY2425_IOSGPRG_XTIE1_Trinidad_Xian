using UnityEngine;
using UnityEngine.UI;

public class GaugeBar : MonoBehaviour
{
    [SerializeField] private Image gaugeBar; 
    [SerializeField] private Image healthBar; 
    [SerializeField] private float maxGaugeAmount = 100f;
    [SerializeField] private float gaugeAmount = 0f;

    [SerializeField] private int maxHealth = 5;
    private int currentHealth;

    private void Start()
    {
        gaugeAmount = 0f;
        currentHealth = maxHealth;
        UpdateGaugeUI();
        UpdateHealthUI();
    }

    public void UpdateHealth(int health)
    {
        currentHealth = health;
        UpdateHealthUI();
    }

    private void UpdateHealthUI()
    {
        healthBar.fillAmount = (float)currentHealth / maxHealth; 
    }

    public void EnemyKilled()
    {
        float amountToAdd = maxGaugeAmount * 0.05f;
        gaugeAmount += amountToAdd;
        gaugeAmount = Mathf.Clamp(gaugeAmount, 0, maxGaugeAmount);
        UpdateGaugeUI();
    }

    public void EnemyKilledWithSpeedBonus()
    {
        float amountToAdd = maxGaugeAmount * 0.10f;
        gaugeAmount += amountToAdd;
        gaugeAmount = Mathf.Clamp(gaugeAmount, 0, maxGaugeAmount);
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
            gaugeAmount = 0f;
            UpdateGaugeUI();
        }
    }

    private void UpdateGaugeUI()
    {
        gaugeBar.fillAmount = gaugeAmount / maxGaugeAmount;
    }
}