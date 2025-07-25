using UnityEngine;
using System.Collections;

public class PlayerStamina : MonoBehaviour
{
    [Header("Stamina Settings")]
    public float maxStamina = 100f;
    public float currentStamina;

    [Header("Costs")]
    public float runStaminaCost = 8f; // 초당
    public float dashCost = 30f;      // 1회

    [Header("Regen")]
    public float staminaRegenRate = 20f; // 초당
    public float regenInterval = 0.1f;

    [Header("Run Settings")]
    public float minStaminaToRun = 30f;

    [HideInInspector] public bool isRunning = false;

    private Coroutine regenCoroutine;
    private Coroutine runDrainCoroutine;

    void Start()
    {
        currentStamina = maxStamina;
        regenCoroutine = StartCoroutine(StaminaRegenRoutine());
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
        return isAlreadyRunning ? currentStamina > 0f : currentStamina >= minStaminaToRun;
    }

    public void StartRunning()
    {
        isRunning = true;
        if (runDrainCoroutine == null)
            runDrainCoroutine = StartCoroutine(RunStaminaDrainRoutine());
    }

    public void StopRunning()
    {
        isRunning = false;
        if (runDrainCoroutine != null)
        {
            StopCoroutine(runDrainCoroutine);
            runDrainCoroutine = null;
        }
    }

    IEnumerator RunStaminaDrainRoutine()
    {
        WaitForSeconds interval = new WaitForSeconds(regenInterval);
        while (true)
        {
            UseStamina(runStaminaCost * regenInterval);
            yield return interval;
        }
    }

    IEnumerator StaminaRegenRoutine()
    {
        WaitForSeconds interval = new WaitForSeconds(regenInterval);
        while (true)
        {
            if (!isRunning && currentStamina < maxStamina)
            {
                currentStamina += staminaRegenRate * regenInterval;
                currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
            }
            yield return interval;
        }
    }

    void UseStamina(float amount)
    {
        currentStamina -= amount;
        currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
    }
}
