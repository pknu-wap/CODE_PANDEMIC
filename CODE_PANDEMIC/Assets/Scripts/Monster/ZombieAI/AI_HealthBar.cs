using UnityEngine;

[RequireComponent(typeof(AI_Health))]
public class AI_HealthBar : MonoBehaviour
{
    [SerializeField] private UI_EnemyStatusBar _statusBar;
    private AI_Health _health;
    private MonsterData _monsterData;

    private void Awake()
    {
        _health = GetComponent<AI_Health>();
    }

    private void OnEnable()
    {
        _health.OnHealthChanged += UpdateHpBar;
        _health.OnDied += HideHpBar;
    }

    private void OnDisable()
    {
        _health.OnHealthChanged -= UpdateHpBar;
        _health.OnDied -= HideHpBar;
    }

    private void Start()
    {
        if (_statusBar != null)
        {
            _statusBar.Init(_health.MonsterData);
        }
    }

    private void UpdateHpBar(int currentHp, int maxHp)
    {
        if (_statusBar != null)
        {
            if (!_statusBar.gameObject.activeSelf)
            {
                _statusBar.gameObject.SetActive(true);
            }
            _statusBar.UpdateHpBar(currentHp);
        }
    }

    private void HideHpBar()
    {
        if (_statusBar != null)
        {
            _statusBar.gameObject.SetActive(false);
        }
    }
}