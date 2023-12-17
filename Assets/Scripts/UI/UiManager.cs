using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UiManager : MonoBehaviour
{
    public static UiManager uiManager;

    public TextMeshProUGUI healthText;
    public TextMeshProUGUI currencyText;

    private void Awake()
    {
        if (uiManager == null)
        {
            uiManager = this;
        }

        else if (uiManager != this)
        {
            Destroy(gameObject);
        }
    }

    // ü�� ������Ʈ
    public void UpdateHealthText(int currentHealth, int maxHealth)
    {
        healthText.text = currentHealth + " / " + maxHealth;
    }

    // ��ȭ ������Ʈ
    public void UpdateCurrencyText(int currency)
    {
        currencyText.text = currency+"��";
    }
}
