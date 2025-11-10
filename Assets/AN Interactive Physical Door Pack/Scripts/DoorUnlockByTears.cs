using UnityEngine;

[DisallowMultipleComponent]
public class DoorUnlockByTears : MonoBehaviour
{
    public AN_DoorScript door;

    [Header("Requisito")]
    [Min(1)] public int requiredTears = 1;

    public enum StartLockMode { None, Locked, RedLocked, BlueLocked }
    [Header("Bloqueo al iniciar")]
    public StartLockMode startLockedMode = StartLockMode.Locked;

    [Header("Desbloqueo")]
    public bool autoPushOnUnlock = true;
    public float pushTorque = 25f;

    private bool done;

    void Reset() { door = GetComponent<AN_DoorScript>(); }

    void Awake()
    {
        if (!door) door = GetComponent<AN_DoorScript>();
        if (door)
        {
            switch (startLockedMode)
            {
                case StartLockMode.Locked: door.Locked = true; break;
                case StartLockMode.RedLocked: door.RedLocked = true; break;
                case StartLockMode.BlueLocked: door.BlueLocked = true; break;
            }
        }
    }

    void Update()
    {
        if (done || GameManager.instance == null || door == null) return;

        if (GameManager.instance.HasTears(requiredTears))
        {
            door.Unlock();
            door.CanOpen = true;

            // SFX de desbloqueo
            var sfx = GetComponent<DoorCreakSFX>();
            if (sfx) sfx.PlayUnlock(transform.position);

            if (autoPushOnUnlock && door.rbDoor)
                door.rbDoor.AddTorque(door.transform.up * pushTorque, ForceMode.Impulse);

            done = true;
        }
    }
}
