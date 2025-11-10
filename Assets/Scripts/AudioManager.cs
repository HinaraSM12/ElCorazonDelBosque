using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Música / Ambiente")]
    public AudioSource musicSource;     // loop música/ambiente 1
    public AudioSource musicSourceB;    // loop música/ambiente 2 (para crossfade)

    [Range(0f, 1f)] public float sfxVolume = 1f;
    [Range(0f, 1f)] public float musicVolume = 0.6f;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        if (!musicSource) musicSource = gameObject.AddComponent<AudioSource>();
        if (!musicSourceB) musicSourceB = gameObject.AddComponent<AudioSource>();

        musicSource.loop = true; musicSource.spatialBlend = 0f; musicSource.volume = musicVolume;
        musicSourceB.loop = true; musicSourceB.spatialBlend = 0f; musicSourceB.volume = 0f;
    }

    /* ================= SFX ================= */
    public AudioSource PlaySFXAt(AudioClip clip, Vector3 pos, float volume = 1f, float pitch = 1f)
    {
        if (!clip) return null;
        var go = new GameObject("SFX_" + clip.name);
        go.transform.position = pos;
        var src = go.AddComponent<AudioSource>();
        src.clip = clip;
        src.spatialBlend = 1f;         // 3D
        src.rolloffMode = AudioRolloffMode.Linear;
        src.minDistance = 2f;
        src.maxDistance = 25f;
        src.volume = sfxVolume * Mathf.Clamp01(volume);
        src.pitch = pitch;
        src.Play();
        Destroy(go, clip.length + 0.1f);
        return src;
    }

    public void Play2D(AudioClip clip, float volume = 1f, float pitch = 1f)
    {
        if (!clip) return;
        var src = gameObject.AddComponent<AudioSource>();
        src.clip = clip; src.spatialBlend = 0f;
        src.volume = sfxVolume * Mathf.Clamp01(volume);
        src.pitch = pitch;
        src.Play();
        Destroy(src, clip.length + 0.1f);
    }

    /* ================= Música / Ambiente ================= */
    public void PlayAmbientLoop(AudioClip loop, float fade = 1f)
    {
        if (!loop) return;
        if (!musicSource.isPlaying)
        {
            musicSource.clip = loop;
            musicSource.volume = 0f;
            musicSource.Play();
            StartCoroutine(FadeTo(musicSource, musicVolume, fade));
        }
        else
        {
            musicSourceB.clip = loop;
            musicSourceB.volume = 0f;
            musicSourceB.Play();
            StartCoroutine(Crossfade(musicSource, musicSourceB, musicVolume, fade));
            // swap
            var tmp = musicSource; musicSource = musicSourceB; musicSourceB = tmp;
        }
    }

    System.Collections.IEnumerator FadeTo(AudioSource src, float target, float t)
    {
        float start = src.volume; float time = 0f;
        while (time < t) { time += Time.deltaTime; src.volume = Mathf.Lerp(start, target, time / t); yield return null; }
        src.volume = target;
    }

    System.Collections.IEnumerator Crossfade(AudioSource from, AudioSource to, float target, float t)
    {
        float startA = from.volume, startB = to.volume; float time = 0f;
        while (time < t)
        {
            time += Time.deltaTime;
            float k = time / t;
            from.volume = Mathf.Lerp(startA, 0f, k);
            to.volume = Mathf.Lerp(startB, target, k);
            yield return null;
        }
        from.Stop(); from.volume = 0f;
        to.volume = target;
    }
}
