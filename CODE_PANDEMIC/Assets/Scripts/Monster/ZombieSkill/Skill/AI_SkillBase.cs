public interface ISkillBehavior
{
    void StartSkill(AI_Controller controller, System.Action onSkillComplete);
    void StopSkill();
    bool IsReady(AI_Controller controller);
}
