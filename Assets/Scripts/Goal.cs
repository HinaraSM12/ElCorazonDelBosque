using System.Collections;
using UnityEngine;

public class GoalWinUI : MonoBehaviour
{
    [Header("Mensaje en UI")]
    [TextArea] public string winText = "¡GANASTE!";
    public string extraText = "\nSaliendo del juego...";
    public float secondsBeforeQuit = 2.0f;

    [Header("Opcional")]
    public AudioClip winSfx;
    public bool freezePlayerUntilQuit = true; // importante: congela sin errores

    private bool done;

    // Detecta Player aunque el trigger esté en un hijo
    private bool IsPlayer(Collider other)
    {
        var go = other.attachedRigidbody ? other.attachedRigidbody.gameObject : other.gameObject;
        return go.transform.root.CompareTag("Player");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (done || !IsPlayer(other)) return;

        var gm = GameManager.instance;
        if (gm == null) return;

        if (!gm.AllTearsCollected())
        {
            UIManager.Instance?.ShowMessage("Aún faltan lágrimas por recolectar...");
            return;
        }

        done = true;
        StartCoroutine(WinAndQuit(other.transform.root.gameObject));
    }

    private IEnumerator WinAndQuit(GameObject playerRoot)
    {
        // Marca fin de nivel
        GameManager.instance.LevelComplete();

        // SFX
        if (winSfx) AudioSource.PlayClipAtPoint(winSfx, transform.position);

        // UI: mostramos en el panel (sin autohide)
        UIManager.Instance?.ShowMessage(winText + extraText, 0f);

        // Congelar control del jugador de forma segura
        if (freezePlayerUntilQuit && playerRoot != null)
        {
            // 1) Desactiva el script de movimiento (evita llamadas a Move())
            var tpc = playerRoot.GetComponent<StarterAssets.ThirdPersonController>();
            if (tpc) tpc.enabled = false;

            // 2) Desactiva el CharacterController
            var cc = playerRoot.GetComponent<CharacterController>();
            if (cc) cc.enabled = false;

            // 3) Si tu jugador tiene Rigidbody, inmovilízalo
            var rb = playerRoot.GetComponent<Rigidbody>();
            if (rb) { rb.linearVelocity = Vector3.zero; rb.isKinematic = true; }
        }

        yield return new WaitForSeconds(secondsBeforeQuit);

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
