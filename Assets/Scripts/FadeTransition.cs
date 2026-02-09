using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Simple fade-to-black transition. Requires a Canvas with a full-screen
/// Image (black, raycast target) and a CanvasGroup on that Image.
/// 
/// Setup:
///   1. Create a Canvas (Screen Space - Overlay, sort order high like 999)
///   2. Add a child Image, stretch to fill, color = black
///   3. Add a CanvasGroup to the Image
///   4. Attach this script to the Image
///   5. Drag it into the TimeSwitch / PlayerSpawner transition slots
/// </summary>
[RequireComponent(typeof(CanvasGroup))]
public class FadeTransition : MonoBehaviour, ISceneTransition
{
    [SerializeField] private float fadeDuration = 0.5f;
    [SerializeField] private AnimationCurve fadeCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

    private CanvasGroup canvasGroup;

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();

        // Start fully transparent so nothing is blocked
        canvasGroup.alpha = 0f;
        canvasGroup.blocksRaycasts = false;
    }

    /// <summary>
    /// Exiting the old scene: fade from clear to black.
    /// </summary>
    public IEnumerator OnExitScene()
    {
        canvasGroup.blocksRaycasts = true;
        yield return Fade(0f, 1f);
    }

    /// <summary>
    /// Optional: update a loading bar, log progress, etc.
    /// </summary>
    public void OnLoadProgress(float normalizedProgress)
    {
        // Could drive a loading bar here.
        // The screen is already black so the player won't see the old scene.
    }

    /// <summary>
    /// Scene is loaded but not yet activated.
    /// We're already fully black, so nothing extra needed.
    /// </summary>
    public IEnumerator OnSceneReady()
    {
        yield break;
    }

    /// <summary>
    /// Entering the new scene: fade from black to clear.
    /// </summary>
    public IEnumerator OnEnterScene()
    {
        // Ensure we start fully black (in case this is a fresh instance in the new scene)
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;

        yield return Fade(1f, 0f);

        canvasGroup.blocksRaycasts = false;
    }

    private IEnumerator Fade(float from, float to)
    {
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(elapsed / fadeDuration);
            canvasGroup.alpha = Mathf.Lerp(from, to, fadeCurve.Evaluate(t));
            yield return null;
        }

        canvasGroup.alpha = to;
    }
}