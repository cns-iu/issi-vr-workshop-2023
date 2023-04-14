using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.XR.Interaction.Toolkit;

public class VignetteApplier : MonoBehaviour
{
    [SerializeField] private float _intensity = .75f;
    [SerializeField] private float _duration = .5f;
    [SerializeField] private Volume _volume = null;
    [SerializeField] private LocomotionProvider _provider = null;

     private Vignette _vignette = null;
    private bool isFading = false;

    private void Awake()
    {

        if (_volume.profile.TryGet(out Vignette vignette))
            this._vignette = vignette;
    }

    private void OnEnable()
    {
        _provider.beginLocomotion += FadeIn;
        _provider.endLocomotion += FadeOut;
    }

    private void OnDestroy()
    {
        _provider.beginLocomotion -= FadeIn;
        _provider.endLocomotion -= FadeOut;
    }

    private void FadeIn(LocomotionSystem system)
    {
        if (isFading) return;
        StartCoroutine(Fade(0f, _intensity));
    }

    private void FadeOut(LocomotionSystem system)
    {
        if (isFading) return;
        StartCoroutine(Fade(_intensity, 0f));
    }

    private IEnumerator Fade(float startValue, float endValue)
    {
        isFading = true;
        float elapsedTime = 0;

        while (elapsedTime <= _duration)
        {
            float blend = elapsedTime / _duration;
            elapsedTime += Time.deltaTime;

            float intensity = Mathf.Lerp(startValue, endValue, blend);
            ApplyValue(intensity);
            yield return null;
        }
        isFading = false;
    }

    private void ApplyValue(float value)
    {
        _vignette.intensity.Override(value);
    }
}
