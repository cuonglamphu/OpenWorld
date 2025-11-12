using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    public Slider healthBar;
    public Button damageButton;
    private float currentHealth = 100f;

    void Start()
    {
        healthBar.value = currentHealth;
        // In ra để debug
        Debug.Log("Máu hiện tại: " + currentHealth);
        // Gán sự kiện cho nút
        damageButton.onClick.AddListener(TakeDamage);
    }

    void TakeDamage()
    {
        currentHealth -= 10f; // giảm 10 máu mỗi lần bấm
        if (currentHealth < 0) currentHealth = 0;

        healthBar.value = currentHealth;


        // In ra để debug
        Debug.Log("Máu hiện tại: " + currentHealth);
    }
}
