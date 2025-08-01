using System;
using UnityEngine;

public interface ISkillBehavior
{
    bool IsReady(AI_Controller controller);
    void StartSkill(AI_Controller controller, Action onSkillComplete);
    void StopSkill();
    void SetController(AI_Controller controller);
    void SetSettings(object settings, LayerMask targetLayer, AI_Controller controller);
}