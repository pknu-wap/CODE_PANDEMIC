using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// 테스트용 스크립트 -> 나중에 데이터로 바꿈

    public class SpawnerData
    {
        public int TemplateID;
        public string SpawnerPrefab;
        public List<MonsterPosData> Monsterss;
    }


[System.Serializable]
public class MonsterPosData
{
    public int ID;
    public Vector2Int Pos;
}
