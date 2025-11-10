using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class AN_HeroController_CC : MonoBehaviour
{
    public float moveSpeed = 6f;
    public float gravity = -9.81f;
    public float jumpHeight = 1.4f;
    public float mouseSensitivity = 120f;

    CharacterController cc;
    Transform cam;
    float yRot, yVel;

    void Start()
    {
        cc = GetComponent<CharacterController>();
        cam = Camera.main.transform;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // ratón
        float mx = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float my = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        transform.Rotate(0f, mx, 0f);
        yRot = Mathf.Clamp(yRot - my, -85f, 60f);
        cam.localRotation = Quaternion.Euler(yRot, 0, 0);

        // movimiento plano
        Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));
        input = Vector3.ClampMagnitude(input, 1f);
        Vector3 move = (transform.right * input.x + transform.forward * input.z) * moveSpeed;

        // salto/gravedad
        if (cc.isGrounded)
        {
            if (yVel < -2f) yVel = -2f;
            if (Input.GetButtonDown("Jump"))
                yVel = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
        yVel += gravity * Time.deltaTime;

        // aplicar
        cc.Move((move + Vector3.up * yVel) * Time.deltaTime);
    }
}
