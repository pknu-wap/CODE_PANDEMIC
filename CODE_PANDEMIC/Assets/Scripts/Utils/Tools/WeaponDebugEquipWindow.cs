#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System;
using System.Collections.Generic;
using Inventory.Model;
using static Define;

public class WeaponDebugEquipWindow : EditorWindow
{
    private GameObject player;
    private GameObject weaponPrefab;

    // 기본 데이터
    private int templateID;
    private int bulletID = 0;
    private int bulletCount = 10;
    private WeaponType weaponType = WeaponType.RangeWeapon;

    // 커스텀 무기 스탯
    private int damage = 10;
    private float fireRate = 0.3f;
    private float bulletSpeed = 20f;
    private float range = 15f;
    private float reloadTime = 1.5f;
    private float spreadAngle = 2f;

    [MenuItem("Tools/Debug/Weapon Equip Tool")]
    public static void ShowWindow()
    {
        GetWindow<WeaponDebugEquipWindow>("Weapon Equip Tool");
    }

    private void OnGUI()
    {
        GUILayout.Label("무기 장착 디버그 툴", EditorStyles.boldLabel);

        player = (GameObject)EditorGUILayout.ObjectField("Player", player, typeof(GameObject), true);
        weaponPrefab = (GameObject)EditorGUILayout.ObjectField("Weapon Prefab", weaponPrefab, typeof(GameObject), false);

        GUILayout.Space(10);
        GUILayout.Label("무기 정보 설정", EditorStyles.boldLabel);

        damage = EditorGUILayout.IntField("Damage", damage);
        fireRate = EditorGUILayout.FloatField("Fire Rate", fireRate);
        bulletSpeed = EditorGUILayout.FloatField("Bullet Speed", bulletSpeed);
        range = EditorGUILayout.FloatField("Range", range);
        reloadTime = EditorGUILayout.FloatField("Reload Time", reloadTime);
        spreadAngle = EditorGUILayout.FloatField("Spread Angle", spreadAngle);

        GUILayout.Space(10);
        GUILayout.Label("추가 설정", EditorStyles.boldLabel);
        bulletID = EditorGUILayout.IntField("Bullet ID", bulletID);
        bulletCount = EditorGUILayout.IntField("Bullet Count", bulletCount);
        weaponType = (WeaponType)EditorGUILayout.EnumPopup("Weapon Type", weaponType);

        GUI.enabled = Application.isPlaying;

        if (GUILayout.Button("플레이 중 장착"))
        {
            TryEquipPrefabWeapon();
        }

        GUI.enabled = true;
    }

    private void TryEquipPrefabWeapon()
    {
        if (!Application.isPlaying)
        {
            Debug.LogWarning("플레이 모드에서만 사용 가능합니다.");
            return;
        }

        if (player == null || weaponPrefab == null)
        {
            Debug.LogWarning("Player 또는 Weapon Prefab이 설정되지 않았습니다.");
            return;
        }

        var equipWeapon = player.GetComponent<EquipWeapon>();
        if (equipWeapon == null)
        {
            Debug.LogError("Player에 EquipWeapon 컴포넌트가 없습니다.");
            return;
        }

        templateID = weaponPrefab.GetInstanceID(); // 임시 ID

        WeaponData data = new WeaponData
        {
            TemplateID = templateID,
            Damage = damage,
            FireRate = fireRate,
            BulletSpeed = bulletSpeed,
            Range = range,
            ReloadTime = reloadTime,
            SpreadAngle = spreadAngle,
            WeaponPrefab = "", // prefab 경로가 필요한 경우 처리
            BulletID = bulletID,
            BulletCount = bulletCount,
            Type = weaponType
        };

        // 데이터 등록
        if (!Managers.Data.Weapons.ContainsKey(templateID))
            Managers.Data.Weapons[templateID] = data;

        WeaponItem item = new WeaponItem { TemplateID = templateID };

        equipWeapon.SetWeaponWithPrefab(item, new List<ItemParameter>(), weaponPrefab);
        Debug.Log("무기 장착 완료");
    }
}
#endif
