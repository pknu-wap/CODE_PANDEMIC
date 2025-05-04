using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_EnlargedMiniMap : UI_Base
{
    private void OnEnable()
    {
        Managers.Event.InvokeEvent("OnMapCamera");
    }
    private void OnDisable()
    {
        Managers.Event.InvokeEvent("OffMapCamera");
    }
}
