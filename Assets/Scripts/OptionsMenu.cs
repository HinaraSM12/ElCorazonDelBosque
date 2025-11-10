using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class OptionsMenu : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private AudioMixer mixer;        // crea un AudioMixer y arrástralo
    [SerializeField] private string masterParam = "MasterVolume"; // expón este parámetro en el mixer
    [SerializeField] private Slider volumeSlider;

    [Header("Vídeo")]
    [SerializeField] private Toggle fullscreenToggle;
    [SerializeField] private TMP_Dropdown qualityDropdown;
    [SerializeField] private TMP_Dropdown resolutionDropdown;

    private Resolution[] _resolutions;

    private void Awake()
    {
        // Volumen
        if (volumeSlider)
        {
            // Lee valor guardado o por defecto 0 dB → slider 1.0
            float db = PlayerPrefs.GetFloat(masterParam, 0f);
            float lin = Mathf.Pow(10f, db / 20f);
            volumeSlider.value = lin;
            SetVolume(lin);
            volumeSlider.onValueChanged.AddListener(SetVolume);
        }

        // Pantalla completa
        if (fullscreenToggle)
        {
            fullscreenToggle.isOn = Screen.fullScreen;
            fullscreenToggle.onValueChanged.AddListener(SetFullscreen);
        }

        // Calidad
        if (qualityDropdown)
        {
            qualityDropdown.ClearOptions();
            qualityDropdown.AddOptions(new System.Collections.Generic.List<string>(QualitySettings.names));
            qualityDropdown.value = QualitySettings.GetQualityLevel();
            qualityDropdown.onValueChanged.AddListener(SetQuality);
        }

        // Resoluciones
        if (resolutionDropdown)
        {
            _resolutions = Screen.resolutions;
            resolutionDropdown.ClearOptions();
            var opts = new System.Collections.Generic.List<string>();
            int currentIndex = 0;
            for (int i = 0; i < _resolutions.Length; i++)
            {
                string label = $"{_resolutions[i].width}x{_resolutions[i].height}@{_resolutions[i].refreshRateRatio.value:F0}";
                opts.Add(label);
                if (_resolutions[i].width == Screen.currentResolution.width &&
                    _resolutions[i].height == Screen.currentResolution.height)
                    currentIndex = i;
            }
            resolutionDropdown.AddOptions(opts);
            resolutionDropdown.value = currentIndex;
            resolutionDropdown.onValueChanged.AddListener(SetResolution);
        }
    }

    public void SetVolume(float linear)
    {
        if (!mixer) return;
        float db = Mathf.Log10(Mathf.Clamp(linear, 0.0001f, 1f)) * 20f;
        mixer.SetFloat(masterParam, db);
        PlayerPrefs.SetFloat(masterParam, db);
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    public void SetQuality(int index)
    {
        QualitySettings.SetQualityLevel(index, true);
    }

    public void SetResolution(int index)
    {
        if (_resolutions == null || index < 0 || index >= _resolutions.Length) return;
        var r = _resolutions[index];
        Screen.SetResolution(r.width, r.height, Screen.fullScreenMode, r.refreshRateRatio);
    }
}
