using UnityEngine;
using System.Linq;

public class AI_Detection : MonoBehaviour
{
    [SerializeField] private AI_Fov _aiFov;
    private Transform _player;
    private bool _attackedPlayer;

    public Transform Player => _player;
    public bool IsPlayerDetected => _attackedPlayer || (_player != null && _aiFov.GetDetectedObjects().Contains(_player.gameObject));

    public void DetectPlayer(System.Action<Transform> onPlayerDetected)
    {
        bool playerFound = false;
        foreach (var obj in _aiFov.GetDetectedObjects())
        {
            if (obj.TryGetComponent<PlayerStatus>(out _))
            {
                _player = obj.transform;
                onPlayerDetected?.Invoke(_player);
                playerFound = true;
                break;
            }
        }

        if (!_attackedPlayer && !playerFound)
        {
            onPlayerDetected?.Invoke(null);
        }
    }

    public void ForceDetectTarget(Transform player)
    {
        _player = player;
        _attackedPlayer = true;
    }
}