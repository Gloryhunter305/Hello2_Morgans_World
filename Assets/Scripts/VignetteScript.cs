using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class VignetteScript : MonoBehaviour
{
    public Volume volume;
    private Vignette vignette;

    void Start()
    {
        if (volume.profile.TryGet<Vignette>(out var vignette))
        {
            this.vignette = vignette;
        }
    }

    void Update()
    {
        // For testing: press V to toggle vignette effect
        // if (Input.GetKeyDown(KeyCode.V))
        // {
        //     if (vignette != null)
        //     {
        //         float target = vignette.intensity.value > 0 ? 0.25f : 0.75f;
        //         StartCoroutine(FadeVignette(1f, target));
        //     }
        // }
    }

    public void StartFadeIn()
    {
        if (vignette != null)
        {
            StartCoroutine(FadeVignette(1, 0.75f));
        }
    }

    public void StartFadeOut()
    {
        if (vignette != null)
        {
            StartCoroutine(FadeVignette(1, 0.25f));
        }
    }

    public IEnumerator FadeVignette(float duration, float targetIntensity)
    {
        if (vignette == null) yield break;

        // current intensity (Vignette.intensity is a ClampedFloatParameter)
        float start = vignette.intensity.value;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            vignette.intensity.value = Mathf.Lerp(start, targetIntensity, t);
            yield return null;
        }

        vignette.intensity.value = targetIntensity;
    }
}
