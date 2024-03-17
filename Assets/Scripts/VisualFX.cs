using System;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class VisualFX : MonoBehaviour
{
    private Volume volume;
    private ChromaticAberration chromaticAberration;
    private Vignette vignette;
    private Damageable playerDam;

    public float targetAberrationIntensity;
    public float distortionShakeTime;
    public float targetVignetteIntensity;
    private float vignetteBaseIntensity;
    private float vignetteShakeOffset;
    public float vignetteShakeAmplitude;
    public float vignetteShakeSpeed;

    private void Start()
    {
        volume = GetComponent<Volume>();
        volume.profile.TryGet(out chromaticAberration);
        volume.profile.TryGet(out vignette);

        playerDam = GameObject.FindWithTag("Player").GetComponent<Damageable>();
        playerDam.onHurt.AddListener(ShakeDistortion);
        playerDam.onHurt.AddListener(SetVignette);
    }

    private void Update()
    {
        vignetteShakeOffset = vignetteShakeAmplitude * Mathf.Sin(Time.time * vignetteShakeSpeed);
        vignette.intensity.Override(vignetteBaseIntensity + vignetteShakeOffset);
    }

    private void ShakeDistortion()
    {
        StopCoroutine(ShakeDistortionCoroutine());
        chromaticAberration.intensity.Override(0f);
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

    private void SetVignette()
    {
        float scaler = (float)(playerDam.maxHealth - playerDam.health) / playerDam.maxHealth;
        // Add an activation function to scaler to make vignette more visible in high health.
        scaler = Mathf.Log(1 + scaler * (math.E - 1));
        vignetteBaseIntensity = scaler * targetVignetteIntensity;
    }
}
