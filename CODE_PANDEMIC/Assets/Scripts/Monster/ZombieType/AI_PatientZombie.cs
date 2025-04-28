 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class AI_PatientZombie : AI_Controller
{
    protected override void Awake()
    {
        _aiName = "PatientZombie";
        _aiHealth = 100;
        _aiDamage = 10f;
        _aiMoveSpeed = 100f;
        _aiDetectionRange = 7.5f;
        _aiDetectionAngle = 120f;
        _aiAttackRange = 5f;
        _aiDamageDelay = 5f;
    }
}
