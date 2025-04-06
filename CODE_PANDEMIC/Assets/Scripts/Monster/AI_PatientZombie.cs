using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_ : AI_Base
{
    void Start()
    {
        _aiName = "PatientZombie";
        _aiHealth = 100f;
        _aiDamage = 10f;
        _aiMoveSpeed = 100f;
        _aiDetectionRange = 7.5f;
        _aiDetectionAngle = 120f;
        _aiDamageDelay = 0.5f;

        if (!Init())
        {
            Debug.LogError("AI 초기화 실패");
            return;
        }
    }

    void Update()
    {
        
    }
}
