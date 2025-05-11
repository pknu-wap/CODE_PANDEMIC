using UnityEngine;

public class PZ_Container : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        _animator.SetBool("IsOpened", true);

        // 여기에 좀비를 소환하는 로직 추가 예정
        // 컨테이너에서 쏟아져 나오는 듯한 연출 넣을 예정

        Destroy(gameObject);
    }
}