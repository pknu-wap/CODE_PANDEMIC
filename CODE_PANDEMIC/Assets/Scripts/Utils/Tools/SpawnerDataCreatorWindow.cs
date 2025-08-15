using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using System;

public class SpawnerDataEditorWindow : EditorWindow
{
    private TextAsset jsonFile;
    private string jsonPath = "";

    private List<SpawnerData> spawners = new List<SpawnerData>(); // 기존 로드된 것
    private List<SpawnerData> newSpawners = new List<SpawnerData>(); // 새로 만든 것
    private Vector2 scroll;
    private bool showExistingSpawners = false;

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

        if (jsonFile == null)
        {
            EditorGUILayout.HelpBox("먼저 JSON 파일을 드래그하세요.", MessageType.Warning);
            return;
        }

        if (spawners == null)
            spawners = new List<SpawnerData>();
        if (newSpawners == null)
            newSpawners = new List<SpawnerData>();

        // 최초 로드
        if (spawners.Count == 0)
        {
            LoadFromJsonAsset(jsonFile);
        }

        // 기존 스포너 표시 토글
        showExistingSpawners = EditorGUILayout.Toggle("Show Existing Spawners", showExistingSpawners);

        scroll = EditorGUILayout.BeginScrollView(scroll);

        // **새로 만든 스포너들 항상 보여줌**
        for (int i = 0; i < newSpawners.Count; i++)
        {
            DrawSpawnerEditor(newSpawners[i], i, isNew: true);
        }

        EditorGUILayout.Space();

        if (showExistingSpawners)
        {
            EditorGUILayout.LabelField("Existing Spawners", EditorStyles.boldLabel);
            for (int i = 0; i < spawners.Count; i++)
            {
                DrawSpawnerEditor(spawners[i], i, isNew: false);
            }
        }

        EditorGUILayout.Space();

        if (GUILayout.Button("Add New Spawner"))
        {
            int nextTemplateID = 1;
            // 새로 만든 게 있으면 그 마지막, 없으면 기존 마지막
            if (newSpawners.Count > 0)
            {
                var last = newSpawners[newSpawners.Count - 1];
                nextTemplateID = last != null ? last.TemplateID + 1 : 1;
            }
            else if (spawners.Count > 0)
            {
                var last = spawners[spawners.Count - 1];
                nextTemplateID = last != null ? last.TemplateID + 1 : 1;
            }

            var newSpawner = new SpawnerData
            {
                TemplateID = nextTemplateID
            };
            newSpawners.Add(newSpawner);
        }

        EditorGUILayout.Space();

        if (GUILayout.Button("Save To JSON"))
        {
            SaveToJson();
        }

        EditorGUILayout.EndScrollView();
    }

    // 공통 렌더 함수
    private void DrawSpawnerEditor(SpawnerData spawner, int index, bool isNew)
    {
        if (spawner.Monsters == null)
            spawner.Monsters = new List<MonsterPosData>();

        EditorGUILayout.BeginVertical("box");
        string label = isNew ? $"New Spawner {index + 1}" : $"Spawner {index + 1}";
        EditorGUILayout.LabelField(label, EditorStyles.boldLabel);

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

        if (GUILayout.Button(isNew ? "Remove This New Spawner" : "Remove This Spawner"))
        {
            if (isNew)
                newSpawners.RemoveAt(index);
            else
                spawners.RemoveAt(index);
        }

        EditorGUILayout.EndVertical();
    }

    private void SaveToJson()
    {
        if (string.IsNullOrEmpty(jsonPath))
        {
            Debug.LogError("저장할 JSON 경로를 찾을 수 없습니다.");
            return;
        }

        // 기존 + 새로 만든 합치기
        var combined = new List<SpawnerData>();
        if (spawners != null)
            combined.AddRange(spawners);
        if (newSpawners != null)
            combined.AddRange(newSpawners);

        var wrapper = new SpawnerListWrapper { spawners = combined };
        string json = JsonUtility.ToJson(wrapper, true);
        File.WriteAllText(jsonPath, json);
        AssetDatabase.Refresh();
        Debug.Log($"Saved to {jsonPath}");

        // 저장 후 새로 만든 건 기존으로 흡수 (선택사항)
        spawners = combined;
        newSpawners.Clear();
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


   
}
[Serializable]
public class SpawnerListWrapper
{
    public List<SpawnerData> spawners = new List<SpawnerData>();
}
