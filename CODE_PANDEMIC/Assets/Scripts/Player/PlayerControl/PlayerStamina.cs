using UnityEngine;
using System.Collections;
using static PlayerStatus;

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

    public delegate void OnStaminaDelegate(float ratio);
    private OnStaminaDelegate _onStaminaUpdate;
    void Start()
    {
        Init();
    }
    private void OnDisable()
    {
        if (Managers.UI.SceneUI is UI_GameScene gameSceneUI && gameSceneUI.StaminaBar != null)
        {
            DisableDelegate(gameSceneUI.StaminaBar.UpdateStamina);
        }
    }
    private void Init()
    {
        currentStamina = maxStamina;
        if (Managers.UI.SceneUI is UI_GameScene gameSceneUI && gameSceneUI.StaminaBar != null)
        {
             SetUpStaminaDelegate(gameSceneUI.StaminaBar.UpdateStamina);
        
        }
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
    private void CallStaminaUpdate(float amount)
    { 
        _onStaminaUpdate(amount);
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
                CallStaminaUpdate(currentStamina / maxStamina);
            }
            
        
            yield return interval;
        }
    }

    void UseStamina(float amount)
    {
        currentStamina -= amount;
        currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
        Debug.Log($"[UseStamina] 사용: {amount}, 현재: {currentStamina}");
        CallStaminaUpdate(currentStamina / maxStamina);
    }
    #region Delegate
    public void SetUpStaminaDelegate(OnStaminaDelegate updateMethod)
    {
        _onStaminaUpdate += updateMethod;
    }
    public void DisableDelegate(OnStaminaDelegate updateMethod)
    {
        _onStaminaUpdate -= updateMethod;
    }
    #endregion
}
