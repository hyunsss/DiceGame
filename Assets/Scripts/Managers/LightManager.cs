using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class LightManager : MonoBehaviour
{
    public Light2D globalLight;
    private Coroutine lightCoroutine;

    void Update()
    {
        if(SceneManager.GetActiveScene().name == "GamePlayScene") {
            if (TimeManager.Instance.IsNightTime() && (lightCoroutine == null || globalLight.intensity > 0))
            {
                if (lightCoroutine != null)
                    StopCoroutine(lightCoroutine);
                lightCoroutine = StartCoroutine(ChangeLightIntensity(globalLight.intensity, 0.1f, 3f));
            }
            else if (TimeManager.Instance.IsDayTime() && (lightCoroutine == null || globalLight.intensity < 1))
            {
                if (lightCoroutine != null)
                    StopCoroutine(lightCoroutine);
                lightCoroutine = StartCoroutine(ChangeLightIntensity(globalLight.intensity, 1f, 3f));
            }
        }
    }

    IEnumerator ChangeLightIntensity(float startIntensity, float endIntensity, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float newIntensity = Mathf.Lerp(startIntensity, endIntensity, elapsed / duration);
            globalLight.intensity = newIntensity;
            yield return null;
        }
        globalLight.intensity = endIntensity;
    }
}
