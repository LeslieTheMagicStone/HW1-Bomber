using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class VisualFX : MonoBehaviour
{
    private Volume volume;
    private ChromaticAberration chromaticAberration;
    private Vignette vignette;

    public float targetAberrationIntensity;
    public float distortionShakeTime;
    public float targetVignetteIntensity;
    public float vignetteShakeTime;

    private void Start()
    {
        volume = GetComponent<Volume>();
        volume.profile.TryGet(out chromaticAberration);
        volume.profile.TryGet(out vignette);

        var playerDam = GameObject.FindWithTag("Player").GetComponent<Damageable>();
        playerDam.onHurt.AddListener(ShakeDistortion);
        playerDam.onHurt.AddListener(ShakeVignette);
    }

    private void ShakeDistortion()
    {
        StopCoroutine(ShakeDistortionCoroutine());
        StartCoroutine(ShakeDistortionCoroutine());
    }

    private IEnumerator ShakeDistortionCoroutine()
    {
        float timer = 0;

        while (timer < distortionShakeTime / 2f)
        {
            float scaler = timer / (distortionShakeTime / 2f);
            chromaticAberration.intensity.Override(targetAberrationIntensity * scaler);
            timer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        while (timer > 0)
        {
            float scaler = timer / (distortionShakeTime / 2f);
            chromaticAberration.intensity.Override(targetAberrationIntensity * scaler);
            timer -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }

    private void ShakeVignette()
    {
        StopCoroutine(ShakeVignetteCoroutine());
        StartCoroutine(ShakeVignetteCoroutine());
    }

    private IEnumerator ShakeVignetteCoroutine()
    {
        float timer = 0;

        while (timer < vignetteShakeTime / 2f)
        {
            float scaler = timer / (vignetteShakeTime / 2f);
            vignette.intensity.Override(targetVignetteIntensity * scaler);
            timer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        while (timer > 0)
        {
            float scaler = timer / (vignetteShakeTime / 2f);
            vignette.intensity.Override(targetVignetteIntensity * scaler);
            timer -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }
}
