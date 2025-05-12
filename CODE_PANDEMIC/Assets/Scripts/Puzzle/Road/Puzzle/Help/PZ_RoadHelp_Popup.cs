using UnityEngine;
using TMPro;

public class PZ_RoadHelp_Popup : UI_PopUp
{
    [SerializeField] private TextMeshProUGUI _text;

    public void SetHelpText(string helpMessage)
    {
        _text.text = helpMessage;
    }
}