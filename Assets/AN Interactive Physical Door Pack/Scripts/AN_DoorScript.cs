using UnityEngine;

public class AN_DoorScript : MonoBehaviour
{
    [Header("Estado")]
    public bool Locked = false;
    public bool Remote = false;

    [Header("Llaves (opcional)")]
    public bool RedLocked = false;
    public bool BlueLocked = false;

    [Header("Compat Pack")]
    public bool isOpened = true;
    public bool CanOpen = true, CanClose = true;

    [Header("Apertura")]
    [Range(5f, 175f)] public float FreeAngle = 135f;
    [Range(0f, 5f)] public float LockedAngle = 0.1f;
    public bool OneSideOnly = true;
    public bool OpenPositive = false;
    [Range(0f, 20f)] public float LimitLerpSpeed = 8f;

    [HideInInspector] public Rigidbody rbDoor;
    private HingeJoint hinge;
    private float currentLim;

    void Awake()
    {
        rbDoor = GetComponent<Rigidbody>();
        hinge = GetComponent<HingeJoint>();
    }

    void Start()
    {
        rbDoor.isKinematic = false;
        rbDoor.useGravity = true;
        if (rbDoor.mass < 12f) rbDoor.mass = 16f;
        if (rbDoor.angularDamping < 0.08f) rbDoor.angularDamping = 0.12f;

        currentLim = IsBlocked() ? LockedAngle : FreeAngle;
        ApplyLimits(currentLim);
        hinge.useLimits = true;
    }

    void FixedUpdate()
    {
        float target = IsBlocked() ? LockedAngle : FreeAngle;
        if (!Mathf.Approximately(currentLim, target))
        {
            currentLim = Mathf.MoveTowards(currentLim, target, LimitLerpSpeed * Time.fixedDeltaTime * 60f);
            ApplyLimits(currentLim);
        }
    }

    public void Action()
    {
        if (Locked) return;
        // aquí solo permitir que se empuje
    }

    public void Unlock() { Locked = false; RedLocked = false; BlueLocked = false; }
    public void Lock() { Locked = true; }

    private bool IsBlocked() => Locked || RedLocked || BlueLocked || !CanOpen;

    private void ApplyLimits(float lim)
    {
        var jl = hinge.limits;
        if (OneSideOnly)
        {
            if (lim <= 0.001f) { jl.min = 0f; jl.max = 0f; }
            else
            {
                if (OpenPositive) { jl.min = 0f; jl.max = lim; }
                else { jl.min = -lim; jl.max = 0f; }
            }
        }
        else { jl.min = -lim; jl.max = lim; }
        hinge.limits = jl;
        hinge.useLimits = true;
    }
}
