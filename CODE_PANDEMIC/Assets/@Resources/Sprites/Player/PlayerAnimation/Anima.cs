using UnityEngine;

public class AnimationController : MonoBehaviour
{
    private Animator anim;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void OnAnimEnd()
    {
        anim.Play("PlayerRun", -1, 0.01f);
    }
}
