using UnityEngine;

public class PlayerStamina : MonoBehaviour
{
    [Header("Stamina Settings")]
    public float maxStamina = 100f;
    public float currentStamina;

    [Header("Costs")]
    public float runStaminaCost = 8f; // 초당
    public float dashCost = 30f;       // 1회

    [Header("Regen")]
    public float staminaRegenRate = 20f; // 초당

    [Header("Run Settings")]
    public float minStaminaToRun = 30f;

    [HideInInspector] public bool isRunning = false;

    void Start()
    {
        currentStamina = maxStamina;
    }

    void Update()
    {
        RegenerateStamina();
    }

    public bool CanDash()
    {
        return currentStamina >= dashCost;
    }

    public void UseDashStamina()
    {
        UseStamina(dashCost);
    }

    public bool CanRun(bool isAlreadyRunning)
    {
        if (isAlreadyRunning)
            return currentStamina > 0f;
        else
            return currentStamina >= minStaminaToRun;
    }

    public void UseRunStamina()
    {
        UseStamina(runStaminaCost * Time.deltaTime);
    }

    void UseStamina(float amount)
    {
        currentStamina -= amount;
        currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
    }

    void RegenerateStamina()
    {
        if (!isRunning && currentStamina < maxStamina)
        {
            currentStamina += staminaRegenRate * Time.deltaTime;
            currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
        }
    }
}
