using UnityEngine;

public class DoorDoctor : MonoBehaviour
{
    public HingeJoint hinge;
    public Rigidbody rb;
    public AN_DoorScript door;

    void Reset()
    {
        hinge = GetComponent<HingeJoint>();
        rb = GetComponent<Rigidbody>();
        door = GetComponent<AN_DoorScript>();
    }

    void OnGUI()
    {
        if (!hinge || !rb || !door) return;

        var jl = hinge.limits;
        string msg =
            $"[DoorDoctor]\n" +
            $"- Locked:{door.Locked}  Red:{door.RedLocked} Blue:{door.BlueLocked}\n" +
            $"- CanOpen:{door.CanOpen}\n" +
            $"- Hinge Axis (local): {hinge.axis}\n" +
            $"- Limits: min={jl.min:F1}  max={jl.max:F1}\n" +
            $"- RB: kinematic={rb.isKinematic} mass={rb.mass} angDrag={rb.angularDamping}\n" +
            $"- Angle:{hinge.angle:F1}";

        GUI.Label(new Rect(20, 20, 600, 120), msg);
    }
}
