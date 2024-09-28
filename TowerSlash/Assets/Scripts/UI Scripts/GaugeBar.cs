using UnityEngine;
using UnityEngine.UI;

public class GaugeBar : MonoBehaviour
{
    [SerializeField] private Image gaugeBar;
    [SerializeField] private float maxGaugeAmount = 100f;
    [SerializeField] private float gaugeAmount = 0f;

    private void Start()
    {
        UpdateGaugeUI();
    }

    public void EnemyKilled()
    {
        float amountToAdd = maxGaugeAmount * 0.05f;
        gaugeAmount += amountToAdd;
        gaugeAmount = Mathf.Clamp(gaugeAmount, 0, maxGaugeAmount);
        UpdateGaugeUI();
    }

    public bool CanDash()
    {
        return gaugeAmount >= maxGaugeAmount; 
    }

    public void UseDash()
    {
        if (CanDash())
        {
            Debug.Log("Dash used!");
            gaugeAmount = 0f; 
            UpdateGaugeUI();
        }
    }

    private void UpdateGaugeUI()
    {
        gaugeBar.fillAmount = gaugeAmount / maxGaugeAmount; 
    }
}
