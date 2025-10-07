using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class SimpleView : MonoBehaviour, IView
{
    [Header("Defaults")] 
    [SerializeField]
    private CanvasGroup canvasGroup;
    [SerializeField]
    private float duration = 0.3f;
    [SerializeField]
    private AnimationCurve curve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    
    private bool useUnscaledTime = true;
    private Coroutine currentCoroutine;

    public virtual void Show() => FadeTo(1f, duration);
    public virtual void Hide() => FadeTo(0f, duration);

    private void FadeTo(float target, float dur)
    {
        if (currentCoroutine != null) StopCoroutine(currentCoroutine);
        if (dur <= 0f)
        {
            SetAlpha(target);
            return;
        }

        currentCoroutine = StartCoroutine(FadeRoutine(target, dur));
    }

    private IEnumerator FadeRoutine(float target, float dur)
    {
        var start = canvasGroup.alpha;
        var t = 0f;

        // while fading in, ensure it can receive input (optional)
        if (target > start) ApplyInteractable(true);

        while (t < 1f)
        {
            var dt = useUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;
            t += dt / Mathf.Max(0.0001f, dur);
            var eased = curve.Evaluate(Mathf.Clamp01(t));
            canvasGroup.alpha = Mathf.LerpUnclamped(start, target, eased);
            yield return null;
        }

        SetAlpha(target);
        currentCoroutine = null;
    }

    private void SetAlpha(float a)
    {
        canvasGroup.alpha = a;
        var visible = a > 0.999f;
        ApplyInteractable(visible);
    }

    private void ApplyInteractable(bool on)
    {
        canvasGroup.interactable = on;
        canvasGroup.blocksRaycasts = on;
    }
}