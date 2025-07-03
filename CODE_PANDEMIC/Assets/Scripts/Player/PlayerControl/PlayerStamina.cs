using UnityEngine;

public class PlayerStamina : MonoBehaviour
{
    public float maxStamina = 10f;
    public float currentStamina;

    public float staminaRegenRate = 10f;
    public float runStaminaCost = 20f;
    public float dodgeCost = 30f;

    public bool isRunning = false;

    void Start()
    {
        currentStamina = maxStamina;
    }

    void Update()
    {
        HandleInput();
        RegenerateStamina();
    }

    void HandleInput()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            isRunning = true;
            UseStamina(runStaminaCost * Time.deltaTime);
        }
        else
        {
            isRunning = false;
        }


        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (currentStamina >= dodgeCost)
            {
                UseStamina(dodgeCost);
            }
            else
            {
                Debug.Log("대쉬 불가 스태니마 부족");
            }
        }
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
