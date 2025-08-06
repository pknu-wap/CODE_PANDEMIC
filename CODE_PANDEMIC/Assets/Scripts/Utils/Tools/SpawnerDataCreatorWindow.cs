using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using System;

public class SpawnerDataEditorWindow : EditorWindow
{
    private TextAsset jsonFile;
    private string jsonPath = "";

    private List<SpawnerData> spawners = new List<SpawnerData>();
    private Vector2 scroll;

    [MenuItem("Tools/Spawner Data Editor")]
    public static void ShowWindow()
    {
        GetWindow<SpawnerDataEditorWindow>("Spawner Data Editor");
    }

    private void OnGUI()
    {
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Step 1: Load JSON File", EditorStyles.boldLabel);
        jsonFile = (TextAsset)EditorGUILayout.ObjectField("JSON File", jsonFile, typeof(TextAsset), false);

        if (spawners == null || spawners.Count == 0)
        {
            if (jsonFile != null)
            {
                LoadFromJsonAsset(jsonFile);
            }
            else
            {
                EditorGUILayout.HelpBox("먼저 JSON 파일을 드래그하세요.", MessageType.Warning);
                return;
            }
        }

        scroll = EditorGUILayout.BeginScrollView(scroll);

        for (int i = 0; i < spawners.Count; i++)
        {
            var spawner = spawners[i];

            EditorGUILayout.BeginVertical("box");
            EditorGUILayout.LabelField($"Spawner {i + 1}", EditorStyles.boldLabel);

            spawner.TemplateID = EditorGUILayout.IntField("Template ID", spawner.TemplateID);

            for (int j = 0; j < spawner.Monsters.Count; j++)
            {
                var monster = spawner.Monsters[j];

                EditorGUILayout.BeginVertical("box");
                monster.ID = EditorGUILayout.IntField("Monster ID", monster.ID);
                monster.Pos = EditorGUILayout.Vector2Field("Position", monster.Pos);

                if (GUILayout.Button("Remove Monster"))
                {
                    spawner.Monsters.RemoveAt(j);
                    break;
                }
                EditorGUILayout.EndVertical();
            }

            if (GUILayout.Button("Add Monster"))
            {
                var newMonster = new MonsterPosData
                {
                    ID = spawner.Monsters.Count + 1,
                    Pos = Vector2.zero
                };
                spawner.Monsters.Add(newMonster);
            }

            if (GUILayout.Button("Remove This Spawner"))
            {
                spawners.RemoveAt(i);
                break;
            }

            EditorGUILayout.EndVertical();
        }

        EditorGUILayout.Space();

        if (GUILayout.Button("Add New Spawner"))
        {
            var newSpawner = new SpawnerData();
            spawners.Add(newSpawner);
        }

        EditorGUILayout.Space();

        if (GUILayout.Button("Save To JSON"))
        {
            SaveToJson();
        }

        EditorGUILayout.EndScrollView();
    }

    private void LoadFromJsonAsset(TextAsset asset)
    {
        try
        {
            string text = asset.text;

            // 만약 최상위가 배열이면 wrapper용으로 감싸기
            if (text.TrimStart().StartsWith("["))
            {
                text = "{\"Spawners\": " + text + "}";
            }

            var wrapper = JsonUtility.FromJson<SpawnerListWrapper>(text);

            if (wrapper == null)
            {
                Debug.LogError("JSON 파싱 실패: wrapper가 null입니다.");
                spawners = new List<SpawnerData>();
                return;
            }

            if (wrapper.spawners == null)
            {
                Debug.LogWarning("JSON 파싱 성공했지만 Spawners 필드가 비어 있습니다. 빈 리스트로 초기화합니다.");
                wrapper.spawners = new List<SpawnerData>();
            }

            // null 방어: 각 SpawnerData 몬스터 리스트가 null일 수 있음
            foreach (var s in wrapper.spawners)
            {
                if (s.Monsters == null)
                    s.Monsters = new List<MonsterPosData>();
            }

            spawners = wrapper.spawners;
            jsonPath = AssetDatabase.GetAssetPath(asset);
            Debug.Log($"Loaded JSON from {jsonPath}, Spawner Count: {spawners.Count}");
        }
        catch (Exception ex)
        {
            Debug.LogError($"JSON 파싱 중 예외 발생: {ex.Message}");
            spawners = new List<SpawnerData>();
        }
    }


    private void SaveToJson()
    {
        if (string.IsNullOrEmpty(jsonPath))
        {
            Debug.LogError("저장할 JSON 경로를 찾을 수 없습니다.");
            return;
        }

        var wrapper = new SpawnerListWrapper { spawners = spawners };
        string json = JsonUtility.ToJson(wrapper, true);
        File.WriteAllText(jsonPath, json);
        AssetDatabase.Refresh();
        Debug.Log($"Saved to {jsonPath}");
    }
}
[Serializable]
public class SpawnerListWrapper
{
    public List<SpawnerData> spawners = new List<SpawnerData>();
}
