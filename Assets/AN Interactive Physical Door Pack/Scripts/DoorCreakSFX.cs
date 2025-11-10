using UnityEngine;

[RequireComponent(typeof(AN_DoorScript))]
[RequireComponent(typeof(HingeJoint))]
public class DoorCreakSFX : MonoBehaviour
{
    [Header("Clips")]
    public AudioClip unlockSfx;   // cuando se desbloquea
    public AudioClip creakLoop;   // bucle suave al moverse
    public AudioClip slamSfx;     // golpe al parar/pegar límite

    [Header("Tuning")]
    public float moveAngularVelThreshold = 2.0f; // cuándo considerar que “se mueve”
    public float slamAngularVelThreshold = 6.0f; // golpe fuerte
    public float minLoopVolume = 0.05f;
    public float maxLoopVolume = 0.6f;

    HingeJoint hinge;
    Rigidbody rb;
    AudioSource loopSrc;
    float lastAngVel;

    void Awake()
    {
        hinge = GetComponent<HingeJoint>();
        rb = GetComponent<Rigidbody>();

        loopSrc = gameObject.AddComponent<AudioSource>();
        loopSrc.loop = true;
        loopSrc.playOnAwake = false;
        loopSrc.spatialBlend = 1f;
        loopSrc.rolloffMode = AudioRolloffMode.Linear;
        loopSrc.minDistance = 2f; loopSrc.maxDistance = 20f;
        loopSrc.volume = 0f; // arrancamos mute
    }

    public void PlayUnlock(Vector3 pos)  // llámalo opcionalmente desde tu DoorUnlockByTears
    {
        if (unlockSfx) AudioManager.Instance?.PlaySFXAt(unlockSfx, pos, 1f);
    }

    void Update()
    {
        if (!rb) return;
        float angVel = rb.angularVelocity.magnitude;

        // CREAK LOOP (volumen según velocidad)
        if (creakLoop)
        {
            if (angVel >= moveAngularVelThreshold)
            {
                if (!loopSrc.isPlaying) { loopSrc.clip = creakLoop; loopSrc.Play(); }
                float t = Mathf.InverseLerp(moveAngularVelThreshold, slamAngularVelThreshold, angVel);
                loopSrc.volume = Mathf.Lerp(minLoopVolume, maxLoopVolume, t);
            }
            else
            {
                if (loopSrc.isPlaying) loopSrc.volume = Mathf.Lerp(loopSrc.volume, 0f, Time.deltaTime * 8f);
                if (loopSrc.isPlaying && loopSrc.volume < 0.01f) loopSrc.Stop();
            }
        }

        // SLAM
        if (slamSfx && lastAngVel >= slamAngularVelThreshold && angVel < 1.0f)
        {
            AudioManager.Instance?.PlaySFXAt(slamSfx, transform.position, 1f);
        }
        lastAngVel = angVel;
    }
}
