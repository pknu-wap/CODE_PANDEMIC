using System.Collections;
using Inventory.Model;
using UnityEngine;
using UnityEngine.UI;

public class UI_ItemSlide : UI_Base
{
    enum Images { ItemImage }
    enum Texts { ItemName }

    RectTransform _rect;
   
    public override bool Init()
    {
        if (base.Init() == false) return false;
        BindImage(typeof(Images));
        BindText(typeof(Texts));
        _rect = gameObject.GetComponent<Image>().rectTransform;
        return true;
    }

    private void OnEnable()
    {
       
    }
    public void SettingImage(Sprite sprite, string name)
    {
       
        GetImage((int)Images.ItemImage).sprite = sprite;
        GetText((int)Texts.ItemName).text = name;
        StartCoroutine(SlideInAndOut());
    }
    IEnumerator SlideInAndOut()
    {
        float moveTime = 0.1f;
        float stayTime = 1.0f;
 
        Vector2 startPos = new Vector2(250f, 0);
        Vector2 midPos = new Vector2(-15f, 0);
       
        _rect.anchoredPosition = startPos;

        float slideTime = 0f;
        while (slideTime < moveTime)
        {
            slideTime += Time.deltaTime;
            float t = Mathf.Clamp01(slideTime / moveTime);
            _rect.anchoredPosition = Vector2.Lerp(startPos, midPos, t);
            yield return null;
        }

        _rect.anchoredPosition = midPos;
        yield return new WaitForSeconds(stayTime);

        slideTime = 0f;
        while (slideTime < moveTime)
        {
            slideTime += Time.deltaTime;
            float t = Mathf.Clamp01(slideTime / moveTime);
            _rect.anchoredPosition = Vector2.Lerp(midPos, startPos, t);
            yield return null;
        }

        Destroy(gameObject);
    }
}
