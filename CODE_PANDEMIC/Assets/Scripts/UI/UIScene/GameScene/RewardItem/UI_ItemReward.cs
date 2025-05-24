using System;
using System.Collections;
using System.Collections.Generic;
using Inventory.Model;
using UnityEngine;

public class UI_ItemReward : UI_Base
{
    Queue<ItemData> _rewardQueue = new Queue<ItemData>();
    bool _isPlaying;
    float _slideInterval = 0.5f; 
   

    public override bool Init()
    {
        if (base.Init() == false) return false;
        _isPlaying = false;

        return true;
    }

    private void OnEnable()
    {
        Managers.Event.Subscribe("ItemReward", OnItemReward);
    }

    private void OnDisable()
    {
        Managers.Event.Unsubscribe("ItemReward", OnItemReward);
    }

    private void OnItemReward(object obj)
    {
        if (obj is ItemData data)
        {
            _rewardQueue.Enqueue(data);

            if (!_isPlaying)
                StartCoroutine(PlayItemSlides());
        }
    }

    private IEnumerator PlayItemSlides()
    {
        _isPlaying = true;
        while (_rewardQueue.Count > 0)
        {
            var data = _rewardQueue.Dequeue(); // 중간에 삭제되는거때문에 queue 사용 

            CreateRewardUI(data);

            yield return CoroutineHelper.WaitForSeconds(_slideInterval);
        }
     
        _isPlaying = false;

    }

    private void CreateRewardUI(ItemData data)
    {
        Managers.Resource.Instantiate("UI_ItemSlide", transform, (obj) =>
        {
            RectTransform imageTransform = obj.GetComponent<RectTransform>();
            imageTransform.pivot = new Vector2(1, 0.5f);
            UI_ItemSlide slide = obj.GetComponent<UI_ItemSlide>();
            slide.Init();
            slide.gameObject.SetActive(false);  //이미지 로드된후에 켜기

            Managers.Resource.LoadAsync<Sprite>(data.Sprite, (sprite) =>
            {
            slide.gameObject.SetActive(true);
            slide.SettingImage(sprite, data.Name);
            });
        });
    }
}
