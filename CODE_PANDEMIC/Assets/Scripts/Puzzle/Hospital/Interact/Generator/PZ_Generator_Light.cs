using UnityEngine;
using System.Collections;
using UnityEngine.Rendering.Universal;

public class PZ_Generator_Light : MonoBehaviour
{
    [SerializeField] private Light2D _light;

    private void Start()
    {
        PZ_Generator.TurnOnGenerator += StartTurnOnLight;
    }

    private void StartTurnOnLight()
    {
        StartCoroutine(FlickerLight());
    }

    private IEnumerator FlickerLight()
    {
        Color originalColor = _light.color;
        Color targetColor = _light.color;

        targetColor.r = 75f / 255f;
        targetColor.g = 75f / 255f;
        targetColor.b = 75f / 255f;

        yield return CoroutineHelper.WaitForSeconds(0.2f);

        _light.color = targetColor;

        yield return  CoroutineHelper.WaitForSeconds(0.5f);

        _light.color = originalColor;

        yield return CoroutineHelper.WaitForSeconds(0.5f);

        _light.color = targetColor;

        yield return CoroutineHelper.WaitForSeconds(0.2f);

        _light.color = originalColor;

        yield return CoroutineHelper.WaitForSeconds(0.2f);

        _light.color = targetColor;
    }

    private void OnDestroy()
    {
        PZ_Generator.TurnOnGenerator -= StartTurnOnLight;
    }
}