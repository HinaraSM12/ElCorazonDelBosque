using UnityEngine;

public class DoorAutoSetup : MonoBehaviour
{
    public bool runOnStart = true;
    public bool openToNegative = true;  // abre hacia el lado negativo por defecto

    void Start()
    {
        if (!runOnStart) return;
        var hinge = GetComponent<HingeJoint>();
        var rb = GetComponent<Rigidbody>();
        var mr = GetComponentInChildren<MeshRenderer>();

        if (!hinge || !rb || !mr) { Debug.LogWarning("[DoorAutoSetup] falta componente"); return; }

        // bounds locales
        var b = mr.bounds;
        // convertimos a espacio local
        Vector3 localCenter = transform.InverseTransformPoint(b.center);
        Vector3 localExtents = transform.InverseTransformVector(b.extents);

        // anchor a la izquierda (x-)
        Vector3 anchor = new Vector3(localCenter.x - localExtents.x, localCenter.y, localCenter.z);
        hinge.anchor = anchor;

        // eje vertical (Y)
        hinge.axis = Vector3.up;

        // límites amplios por si acaso
        var jl = hinge.limits;
        jl.min = openToNegative ? -135f : 0f;
        jl.max = openToNegative ? 0f : 135f;
        hinge.limits = jl;
        hinge.useLimits = true;

        Debug.Log("[DoorAutoSetup] eje=Y, anchor colocado, límites ±135 ajustados.");
    }
}
