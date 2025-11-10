using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("Panels (opcionales)")]
    [SerializeField] private GameObject optionsPanel;
    [SerializeField] private GameObject creditsPanel;

    [Header("Escenas")]
    [SerializeField] private string gameSceneName = "Game";
    [SerializeField] private string loadingSceneName = "Loading"; // opcional

    [Header("Transición (opcional)")]
    [SerializeField] private CanvasGroup fadeCanvas;  // un CanvasGroup negro a pantalla completa
    [SerializeField] private float fadeDuration = 0.4f;

    private bool _busy;

    private void Start()
    {
        // Asegura estado inicial
        if (optionsPanel) optionsPanel.SetActive(false);
        if (creditsPanel) creditsPanel.SetActive(false);

        if (fadeCanvas)
        {
            fadeCanvas.blocksRaycasts = false;
            fadeCanvas.alpha = 0f; // visible al salir si quieres animación inversa
        }
    }

    public void OnPlayPressed()
    {
        if (_busy) return;
        _busy = true;
        if (string.IsNullOrEmpty(loadingSceneName))
            StartCoroutine(LoadGameDirect());
        else
            StartCoroutine(LoadViaLoadingScene());
    }

    public void OnOptionsPressed()
    {
        if (optionsPanel) optionsPanel.SetActive(true);
    }

    public void OnCreditsPressed()
    {
        if (creditsPanel) creditsPanel.SetActive(true);
    }

    public void OnCloseSubpanel(GameObject panel)
    {
        if (panel) panel.SetActive(false);
    }

    public void OnQuitPressed()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    // ====== Carga directa (sin pantalla Loading) ======
    private System.Collections.IEnumerator LoadGameDirect()
    {
        yield return Fade(1f);
        var op = SceneManager.LoadSceneAsync(gameSceneName, LoadSceneMode.Single);
        while (!op.isDone) yield return null;
    }

    // ====== Carga con escena "Loading" ======
    private System.Collections.IEnumerator LoadViaLoadingScene()
    {
        yield return Fade(1f);
        SceneManager.LoadScene(loadingSceneName, LoadSceneMode.Single);
        // La escena Loading llamará a SceneLoader.LoadTargetAsync(...) (ver paso 5)
    }

    private System.Collections.IEnumerator Fade(float target)
    {
        if (!fadeCanvas) yield break;
        fadeCanvas.blocksRaycasts = true;
        float t = 0f, start = fadeCanvas.alpha;
        while (t < fadeDuration)
        {
            t += Time.unscaledDeltaTime;
            fadeCanvas.alpha = Mathf.Lerp(start, target, t / fadeDuration);
            yield return null;
        }
        fadeCanvas.alpha = target;
        fadeCanvas.blocksRaycasts = target > 0.99f;
    }
}
