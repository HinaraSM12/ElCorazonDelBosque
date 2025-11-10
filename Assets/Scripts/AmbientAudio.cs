using UnityEngine;

public class AmbientAudio : MonoBehaviour
{
    public AudioClip ambientLoop;
    public float fadeSeconds = 1.5f;

    void Start()
    {
        if (AudioManager.Instance && ambientLoop)
            AudioManager.Instance.PlayAmbientLoop(ambientLoop, fadeSeconds);
    }
}
