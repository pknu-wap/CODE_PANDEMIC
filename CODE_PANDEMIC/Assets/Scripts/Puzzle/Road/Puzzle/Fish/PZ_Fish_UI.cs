using UnityEngine;
using TMPro;
using System.Collections;
using System;

public class PZ_Fish_UI : UI_PopUp
{
    [SerializeField] private GameObject _mainText;
    [SerializeField] private TextMeshProUGUI _timeText;

    public static event Action FishTimeOver;

    private int _currentTime = 60;

    public void CloseMainText()
    {
        _mainText.SetActive(false);
    }

    public IEnumerator FishFindTimer()
    {
        while (_currentTime >= 0)
        {
            yield return new WaitForSeconds(1);

            _timeText.text = _currentTime.ToString();

            _currentTime--;
        }

        FishTimeOver?.Invoke();
    }
}