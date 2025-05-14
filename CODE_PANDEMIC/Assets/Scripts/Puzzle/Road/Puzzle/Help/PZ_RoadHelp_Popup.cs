using UnityEngine;
using TMPro;

public class PZ_RoadHelp_Popup : UI_PopUp
{
    [SerializeField] private TextMeshProUGUI _text;

    private PZ_RoadHelp _helpSign;

    public void SetHelpText(PZ_RoadHelp helpSign, string helpMessage)
    {
        _helpSign = helpSign;
        _text.text = helpMessage;
    }

    private void OnDisable()
    {
        _helpSign.SetInteractable();
    }
}