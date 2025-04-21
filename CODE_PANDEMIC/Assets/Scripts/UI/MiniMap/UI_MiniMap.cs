using UnityEngine.UI;
using UnityEngine;

public class UI_MiniMap : UI_Base
{
    enum GameObjects { MiniMap }

    RawImage _rawImage;

    public override bool Init()
    {
        if (!base.Init()) return false;
        BindObject(typeof(GameObjects));
        _rawImage = GetObject((int)GameObjects.MiniMap).GetComponent<RawImage>();

        Managers.Event.Subscribe("OnMiniMapReady", OnMiniMapReady);
        return true;
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
