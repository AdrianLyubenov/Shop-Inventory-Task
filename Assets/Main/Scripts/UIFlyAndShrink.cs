using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIFlyAndShrink : MonoBehaviour
{
    [SerializeField]
    private RectTransform ui;
    [SerializeField]
    private Image image;
    [SerializeField]
    private TextMeshProUGUI text;
    [SerializeField]
    private CanvasGroup canvasGroup;
    [SerializeField]
    private float duration = 0.6f;
    [SerializeField]
    private AnimationCurve ease = AnimationCurve.EaseInOut(0, 0, 1, 1);

    private Coroutine current;

    private void Start()
    {
        ServiceLocator.Instance.Get<InventoryService>().onChanged += OnItemPurchased;
    }

    private void OnItemPurchased(BaseInventoryItemSO intemSo, int quantity)
    {
        canvasGroup.alpha = 1;
        ui.anchoredPosition = Vector3.zero;
        ui.localScale = Vector3.one;
        image.sprite = intemSo.icon;
        text.text = intemSo.name;
        MoveAndShrinkAnchored(new Vector2(300, -300), new Vector3(0, 0, 0));
    }

    public void MoveAndShrinkAnchored(Vector2 targetAnchoredPos, Vector3 targetScale, float? overrideDuration = null)
    {
        if (current != null) StopCoroutine(current);
        current = StartCoroutine(CoMoveAndShrinkAnchored(targetAnchoredPos, targetScale, overrideDuration ?? duration));
    }

    public void MoveAndShrinkToWorld(Transform worldTarget, Camera worldCam, Canvas canvas, Vector3 targetScale,
        float? overrideDuration = null)
    {
        if (current != null) StopCoroutine(current);
        current = StartCoroutine(CoMoveAndShrinkWorld(worldTarget, worldCam, canvas, targetScale,
            overrideDuration ?? duration));
    }

    private IEnumerator CoMoveAndShrinkAnchored(Vector2 targetPos, Vector3 targetScale, float dur)
    {
        var startPos = ui.anchoredPosition;
        var startScale = ui.localScale;
        var t = 0f;

        while (t < dur)
        {
            t += Time.unscaledDeltaTime; // use unscaled so UI animates during pauses; use deltaTime if you prefer
            var k = ease.Evaluate(Mathf.Clamp01(t / dur));

            ui.anchoredPosition = Vector2.LerpUnclamped(startPos, targetPos, k);
            ui.localScale = Vector3.LerpUnclamped(startScale, targetScale, k);

            if (canvasGroup) canvasGroup.alpha = Mathf.LerpUnclamped(1f, 0.6f, k); // optional fade
            yield return null;
        }

        ui.anchoredPosition = targetPos;
        ui.localScale = targetScale;
        if (canvasGroup) canvasGroup.alpha = 0.6f;
        current = null;
    }

    private IEnumerator CoMoveAndShrinkWorld(Transform worldTarget, Camera worldCam, Canvas canvas,
        Vector3 targetScale, float dur)
    {
        var startPos = ui.anchoredPosition;
        var startScale = ui.localScale;

        var t = 0f;

        while (t < dur)
        {
            t += Time.unscaledDeltaTime;
            var k = ease.Evaluate(Mathf.Clamp01(t / dur));

            Vector2 canvasPoint;
            var screenPoint = worldCam.WorldToScreenPoint(worldTarget.position);
            var canvasRect = canvas.transform as RectTransform;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenPoint,
                canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : worldCam, out canvasPoint);

            var targetAnchored = canvasPoint; // anchored relative to canvasRect pivot
            ui.anchoredPosition = Vector2.LerpUnclamped(startPos, targetAnchored, k);
            ui.localScale = Vector3.LerpUnclamped(startScale, targetScale, k);

            if (canvasGroup) canvasGroup.alpha = Mathf.LerpUnclamped(1f, 0.6f, k);
            yield return null;
        }

        if (canvasGroup) canvasGroup.alpha = 0.6f;
        current = null;
    }

    private void OnDisable()
    {
        StopAllCoroutines();
        ServiceLocator.Instance.Get<InventoryService>().onChanged -= OnItemPurchased;
    }
}