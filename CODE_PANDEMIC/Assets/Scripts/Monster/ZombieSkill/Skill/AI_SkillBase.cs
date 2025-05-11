public interface ISkillBehavior
{
    void SetController(AI_Controller controller);
    void StartSkill(AI_Controller controller, System.Action onSkillComplete);
    void StopSkill();
    bool IsReady(AI_Controller controller);
}
