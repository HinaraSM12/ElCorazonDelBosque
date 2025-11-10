using UnityEngine;

public class DoorMessageTrigger : MonoBehaviour
{
    public int requiredTears = 1;
    [TextArea] public string messagePrefix = "Necesitas";
    [TextArea] public string messageReady = "¡Listo! Empuja la puerta.";

    bool IsPlayer(Collider other)
    {
        var go = other.attachedRigidbody ? other.attachedRigidbody.gameObject : other.gameObject;
        return go.transform.root.CompareTag("Player");
    }

    void OnTriggerEnter(Collider other)
    {
        if (!IsPlayer(other)) return;
        var gm = GameManager.instance;
        if (!gm) return;

        if (gm.HasTears(requiredTears))
            UIManager.Instance?.ShowMessage(messageReady, 2f);
        else
        {
            int faltan = Mathf.Max(0, requiredTears - gm.CurrentTears);
            string msg = $"{messagePrefix} {requiredTears} lágrima(s). Faltan {faltan}.";
            UIManager.Instance?.ShowMessage(msg);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (!IsPlayer(other)) return;
        UIManager.Instance?.HideMessage();
    }
}
