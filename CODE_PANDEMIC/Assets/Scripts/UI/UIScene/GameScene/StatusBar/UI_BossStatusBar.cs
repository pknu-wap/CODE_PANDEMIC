using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_BossStatusBar : UI_EnemyStatusBar
{
    [SerializeField]
    private TextMeshProUGUI _bossName;

    public override void Init(MonsterData monsterData)
    {
        base.Init(monsterData);
        _bossName.text = monsterData.NameID;
    }



}
