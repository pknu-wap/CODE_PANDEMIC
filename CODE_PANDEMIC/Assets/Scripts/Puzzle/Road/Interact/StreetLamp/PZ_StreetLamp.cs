using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PZ_StreetLamp : PZ_Interact_NonSpawn
{
    [SerializeField] private Animator _animator;
    [SerializeField] private bool _isLeft = true;
    [SerializeField] private Light2D _pointLight;

    public override void Interact(GameObject player)
    {
        if (_isInteracted)
        {
            return;
        }

        base.Interact(player);

        //if (_isLeft)
        //{
        //    Vector3 target = transform.position;
        //    target.x += 0.5f;
        //    transform.position = target;
        //}
        //else
        //{
        //    Vector3 target = transform.position;
        //    target.x -= 0.5f;
        //    transform.position = target;
        //}

        _animator.SetBool("IsTurnOn", true);

        Color change = _pointLight.color;
        change.a = 1;
        _pointLight.color = change;
    }
}