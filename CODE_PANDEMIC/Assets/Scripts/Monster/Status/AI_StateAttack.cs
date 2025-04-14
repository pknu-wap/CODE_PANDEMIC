using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class AI_StateAttack : AI_IState
{
    private readonly AI_Controller _controller;
    private bool _isSkillPlaying;

    public AI_StateAttack(AI_Controller controller)
    {
        _controller = controller;
    }

    public void OnEnter()
    {
        _controller.StopMoving();
        Debug.Log($"[StateAttack] 공격 상태 진입");
        // 시작 시 항상 스킬 실행 (공격 중에는 상태 전환 안 함)
        _isSkillPlaying = true;
        if (_controller is AI_DoctorZombie doctor && doctor.Skill != null)
        {
            doctor.Skill.StartSkill(_controller, OnSkillComplete);
        }
        else
        {
            OnSkillComplete();
        }
    }

    public void OnUpdate()
    {
        // 스킬이 진행 중이면 상태 전환하지 않음
        if (_isSkillPlaying)
            return;
        // 만약 추가 조건이 있다면 여기서 처리
    }

    public void OnFixedUpdate() { }

    public void OnExit()
    {
        if (_controller is AI_DoctorZombie doctor && doctor.Skill != null)
        {
            doctor.Skill.StopSkill();
        }
        Debug.Log($"[StateAttack] 공격 상태 종료");
    }

    private void OnSkillComplete()
    {
        _isSkillPlaying = false;
        _controller.ChangeState(new AI_StateIdle(_controller));
    }
}