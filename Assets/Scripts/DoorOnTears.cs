using UnityEngine;

public class DoorOnTears : MonoBehaviour
{
    public AN_DoorScript door;
    public GameManager gm;
    public bool openOnce = true;
    private bool done;

    void Update()
    {
        if (done || door == null || gm == null) return;

        if (gm.AllTearsCollected())
        {
            // Desbloquea totalmente la puerta y permite abrir
            door.Unlock();
            door.CanOpen = true;

            // (Opcional) darle un pequeño empujón para que empiece a abrir
            if (door.rbDoor != null)
                door.rbDoor.AddTorque(door.transform.up * 25f, ForceMode.Impulse);

            done = openOnce;
        }
    }
}
