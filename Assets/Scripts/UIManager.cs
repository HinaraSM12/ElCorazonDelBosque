using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("Mensajes (opcional)")]
    public GameObject messagePanel;
    public TMP_Text messageTextTMP;   // si usas TextMeshPro
    public Text messageTextUI;        // fallback con UI.Text

    [Header("HUD Lágrimas")]
    public GameObject tearsPanel;     // arrastra tu TearsPanel
    public TMP_Text tearsTextTMP;     // arrastra TearsText (TMP) si usas TMP
    public Text tearsTextUI;          // o UI.Text si no usas TMP

    private Coroutine hideCo;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;

        // Crea Canvas/Paneles/Textos si no están asignados
        EnsureRuntimeUIExists();
    }

    /*==================== HUD LÁGRIMAS ====================*/
    public void SetTears(int current, int total)
    {
        string t = $"Lágrimas: {current}/{total}";

        if (tearsTextTMP != null) tearsTextTMP.text = t;
        if (tearsTextUI != null) tearsTextUI.text = t;

        if (tearsPanel != null && !tearsPanel.activeSelf)
            tearsPanel.SetActive(true);
    }

    /*==================== MENSAJES ====================*/
    public void ShowMessage(string text, float autoHideSeconds = 0f)
    {
        if (messageTextTMP != null) messageTextTMP.text = text;
        if (messageTextUI != null) messageTextUI.text = text;

        if (messagePanel != null && !messagePanel.activeSelf)
            messagePanel.SetActive(true);

        if (hideCo != null) StopCoroutine(hideCo);
        if (autoHideSeconds > 0f)
            hideCo = StartCoroutine(AutoHide(autoHideSeconds));
    }

    public void HideMessage()
    {
        if (hideCo != null) StopCoroutine(hideCo);
        if (messagePanel != null && messagePanel.activeSelf)
            messagePanel.SetActive(false);
    }

    private IEnumerator AutoHide(float t)
    {
        yield return new WaitForSeconds(t);
        HideMessage();
    }

    /*==================== HELPERS (CREACIÓN RUNTIME) ====================*/
    private void EnsureRuntimeUIExists()
    {
        // 1) Canvas
        Canvas canvas = null;
        if (messagePanel != null) canvas = messagePanel.GetComponentInParent<Canvas>();
        if (canvas == null && tearsPanel != null) canvas = tearsPanel.GetComponentInParent<Canvas>();
#if UNITY_2023_1_OR_NEWER
        if (canvas == null) canvas = Object.FindFirstObjectByType<Canvas>(FindObjectsInactive.Include);
#else
        if (canvas == null) canvas = FindObjectOfType<Canvas>();
#endif
        if (canvas == null)
        {
            var canvasGO = new GameObject("Canvas (Runtime)");
            canvas = canvasGO.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            var scaler = canvasGO.AddComponent<CanvasScaler>();
            canvasGO.AddComponent<GraphicRaycaster>();

            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920, 1080);
            scaler.matchWidthOrHeight = 0.5f;
        }

        // 2) Mensajes: Panel + Texto
        if (messagePanel == null)
        {
            var panelGO = new GameObject("MessagePanel", typeof(RectTransform), typeof(Image));
            panelGO.transform.SetParent(canvas.transform, false);
            var rt = panelGO.GetComponent<RectTransform>();
            rt.sizeDelta = new Vector2(900, 140);
            rt.anchorMin = new Vector2(0.5f, 0.15f);
            rt.anchorMax = new Vector2(0.5f, 0.15f);
            rt.pivot = new Vector2(0.5f, 0.5f);
            var img = panelGO.GetComponent<Image>();
            img.color = new Color(0f, 0f, 0f, 0.6f);
            messagePanel = panelGO;
        }
        if (messageTextTMP == null && messageTextUI == null)
        {
            var textGO = new GameObject("MessageTextTMP", typeof(RectTransform));
            textGO.transform.SetParent(messagePanel.transform, false);
            var rt = textGO.GetComponent<RectTransform>();
            rt.anchorMin = Vector2.zero; rt.anchorMax = Vector2.one;
            rt.offsetMin = new Vector2(20, 20); rt.offsetMax = new Vector2(-20, -20);

            messageTextTMP = textGO.AddComponent<TextMeshProUGUI>();
            messageTextTMP.alignment = TextAlignmentOptions.Center;
            messageTextTMP.fontSize = 36;
            messageTextTMP.color = Color.white;

            if (messageTextTMP == null)
            {
                var textUGO = new GameObject("MessageText", typeof(RectTransform));
                textUGO.transform.SetParent(messagePanel.transform, false);
                var rt2 = textUGO.GetComponent<RectTransform>();
                rt2.anchorMin = Vector2.zero; rt2.anchorMax = Vector2.one;
                rt2.offsetMin = new Vector2(20, 20); rt2.offsetMax = new Vector2(-20, -20);

                messageTextUI = textUGO.AddComponent<Text>();
                messageTextUI.alignment = TextAnchor.MiddleCenter;
                messageTextUI.fontSize = 28;
                messageTextUI.color = Color.white;
                if (messageTextUI.font == null)
                    messageTextUI.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            }
        }
        if (messagePanel != null) messagePanel.SetActive(false);

        // 3) Tears: Panel + Texto
        if (tearsPanel == null)
        {
            var panelGO = new GameObject("TearsPanel", typeof(RectTransform), typeof(Image));
            panelGO.transform.SetParent(canvas.transform, false);
            var rt = panelGO.GetComponent<RectTransform>();
            rt.sizeDelta = new Vector2(240, 80);
            rt.anchorMin = new Vector2(0f, 1f);
            rt.anchorMax = new Vector2(0f, 1f);
            rt.pivot = new Vector2(0f, 1f);
            rt.anchoredPosition = new Vector2(20, -20);
            var img = panelGO.GetComponent<Image>();
            img.color = new Color(0f, 0f, 0f, 0.35f);
            tearsPanel = panelGO;
        }
        if (tearsTextTMP == null && tearsTextUI == null)
        {
            var textGO = new GameObject("TearsTextTMP", typeof(RectTransform));
            textGO.transform.SetParent(tearsPanel.transform, false);
            var rt = textGO.GetComponent<RectTransform>();
            rt.anchorMin = Vector2.zero; rt.anchorMax = Vector2.one;
            rt.offsetMin = new Vector2(16, 12); rt.offsetMax = new Vector2(-16, -12);

            tearsTextTMP = textGO.AddComponent<TextMeshProUGUI>();
            tearsTextTMP.alignment = TextAlignmentOptions.MidlineLeft;
            tearsTextTMP.fontSize = 36;
            tearsTextTMP.color = Color.white;

            if (tearsTextTMP == null)
            {
                var textUGO = new GameObject("TearsText", typeof(RectTransform));
                textUGO.transform.SetParent(tearsPanel.transform, false);
                var rt2 = textUGO.GetComponent<RectTransform>();
                rt2.anchorMin = Vector2.zero; rt2.anchorMax = Vector2.one;
                rt2.offsetMin = new Vector2(16, 12); rt2.offsetMax = new Vector2(-16, -12);

                tearsTextUI = textUGO.AddComponent<Text>();
                tearsTextUI.alignment = TextAnchor.MiddleLeft;
                tearsTextUI.fontSize = 28;
                tearsTextUI.color = Color.white;
                if (tearsTextUI.font == null)
                    tearsTextUI.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            }
        }
        if (tearsPanel != null) tearsPanel.SetActive(false); // se enciende con SetTears
    }
}
