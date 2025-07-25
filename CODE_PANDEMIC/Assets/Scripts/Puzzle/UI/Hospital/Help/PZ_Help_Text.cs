using UnityEngine;
using TMPro;

public class PZ_Help_Text : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _textMeshProUGUI;

    private bool _onHint = false;

    public void Setting(string hintText)
    {
        _textMeshProUGUI.text = hintText;

        gameObject.SetActive(false);
    }

    public void ToggleHint()
    {
        gameObject.SetActive(!_onHint);

        _onHint = !_onHint;
    }
}