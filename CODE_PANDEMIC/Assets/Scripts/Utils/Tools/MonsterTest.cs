using UnityEditor;
using UnityEngine;

public class MonsterTestWindow : EditorWindow
{
    private GameObject monsterPrefab;
    private MonsterData monsterData = new MonsterData();
    private Vector3 spawnPos = Vector3.zero;

    [MenuItem("Tools/Monster Test Window")]
    public static void ShowWindow()
    {
        GetWindow<MonsterTestWindow>("Monster Test");
    }

    private void OnGUI()
    {
        GUILayout.Label("Monster Test Spawner", EditorStyles.boldLabel);

        monsterPrefab = (GameObject)EditorGUILayout.ObjectField("Monster Prefab", monsterPrefab, typeof(GameObject), false);

        GUILayout.Label("Monster Data", EditorStyles.boldLabel);
        monsterData.NameID = EditorGUILayout.TextField("Name ID", monsterData.NameID);
        monsterData.Hp = EditorGUILayout.IntField("HP", monsterData.Hp);
        monsterData.AttackDamage = EditorGUILayout.IntField("Attack Damage", monsterData.AttackDamage);
        monsterData.AttackDelay = EditorGUILayout.FloatField("Attack Delay", monsterData.AttackDelay);
        monsterData.DetectionRange = EditorGUILayout.FloatField("Detection Range", monsterData.DetectionRange);
        monsterData.DetectionAngle = EditorGUILayout.FloatField("Detection Angle", monsterData.DetectionAngle);
        monsterData.MoveSpeed = EditorGUILayout.FloatField("Move Speed", monsterData.MoveSpeed);
        monsterData.AttackRange = EditorGUILayout.FloatField("Attack Range", monsterData.AttackRange);

        spawnPos = EditorGUILayout.Vector3Field("Spawn Position", spawnPos);

        GUI.enabled = Application.isPlaying;
        if (GUILayout.Button("Spawn Monster"))
        {
            SpawnMonster();
        }
        GUI.enabled = true;
    }

    private void SpawnMonster()
    {
        if (monsterPrefab == null)
        {
            Debug.LogWarning("Monster Prefab is not assigned.");
            return;
        }

        GameObject obj = Instantiate(monsterPrefab, spawnPos, Quaternion.identity);
        AI_Base ai = obj.GetComponent<AI_Base>();
        if (ai != null)
        {
            ai.SetInfo(monsterData);
            Debug.Log($"Spawned monster '{monsterData.NameID}' with HP: {monsterData.Hp}");
        }
        else
        {
            Debug.LogError("Prefab does not have AI_Base component.");
        }
    }
}
