using UnityEngine.UI;
using UnityEngine;
using System;

public class UI_MiniMap : UI_Base
{
    enum GameObjects 
    { 
        MiniMap 
    }
    enum Buttons 
    {
        MinimapButton
    }
    RawImage _rawImage;
    [SerializeField]
    GameObject _miniMapButton;
    public override bool Init()
    {
        if (!base.Init()) return false;
        BindObject(typeof(GameObjects));
        BindButton(typeof(Buttons));

        _rawImage = GetObject((int)GameObjects.MiniMap).GetComponent<RawImage>();

        _miniMapButton = GetButton((int)Buttons.MinimapButton).gameObject;

        BindEvent(_miniMapButton, OnClickMiniMapButton);
        Managers.Event.Subscribe("OnMiniMapReady", OnMiniMapReady);
        return true;
    }

    private void OnClickMiniMapButton()
    {
        Debug.Log("click");
        if (Managers.UI.EnlargedMiniMapUI != null)
        {
            Managers.UI.CloseEnlargedMiniMap();
        }
        else
        Managers.UI.OpenEnlargedMiniMap();
    }

    void OnDestroy()
    {
        Managers.Event.Unsubscribe("OnMiniMapReady", OnMiniMapReady);
    }

    void OnMiniMapReady(object param)
    {
        RenderTexture texture = param as RenderTexture;
        if (texture != null)
            _rawImage.texture = texture;
    }
}
