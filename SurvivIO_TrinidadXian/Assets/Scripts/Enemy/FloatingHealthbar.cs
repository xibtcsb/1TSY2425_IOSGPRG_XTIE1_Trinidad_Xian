using UnityEngine;
using UnityEngine.UI;

public class FloatingHealthbar : MonoBehaviour
{
    [SerializeField] private Slider _slider;

    public void UpdateHealthBar(float currentHp, float maxHp)
    {
        if (_slider != null)
        {
            _slider.value = currentHp / maxHp;
        }
    }
}
