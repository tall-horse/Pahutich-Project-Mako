using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
namespace Mako.State
{
    public class Settings : MonoBehaviour
    {
        private const string exposedGeneralParameterName = "masterVolume";
        private const string exposedSfxParameterName = "sfxVolume";
        private const string exposedMusicVolumeParameterName = "musicVolume";
        [SerializeField] private AudioMixer mixer;
        [SerializeField] private Slider generalVolumeSlider;
        [SerializeField] private Slider sfxVolumeSlider;
        [SerializeField] private Slider musicVolumeSlider;
        private void Start()
        {
            SetupInitialSliderValue(exposedGeneralParameterName, ref generalVolumeSlider);
            SetupInitialSliderValue(exposedSfxParameterName, ref sfxVolumeSlider);
            SetupInitialSliderValue(exposedMusicVolumeParameterName, ref musicVolumeSlider);
        }

        private void SetupInitialSliderValue(string parameterName, ref Slider sliderToSetup)
        {
            float sliderStartValue;
            float savedValue = PlayerPrefs.GetFloat(parameterName, 0);
            mixer.SetFloat(parameterName, savedValue);
            sliderStartValue = Mathf.Exp(savedValue / 20);
            sliderToSetup.value = sliderStartValue;
        }

        public void ConfigureGeneralVolume()
        {
            float newValue = Mathf.Log(generalVolumeSlider.value) * 20f;
            mixer.SetFloat(exposedGeneralParameterName, newValue);
            PlayerPrefs.SetFloat(exposedGeneralParameterName, newValue);
        }
        public void ConfigureSFXVolume()
        {
            float newValue = Mathf.Log(sfxVolumeSlider.value) * 20f;
            mixer.SetFloat(exposedSfxParameterName, newValue);
            PlayerPrefs.SetFloat(exposedSfxParameterName, newValue);
        }
        public void ConfigureMusicVolume()
        {
            float newValue = Mathf.Log(musicVolumeSlider.value) * 20f;
            mixer.SetFloat(exposedMusicVolumeParameterName, newValue);
            PlayerPrefs.SetFloat(exposedMusicVolumeParameterName, newValue);
        }
        public void ResetToDefaults()
        {
            PlayerPrefs.DeleteKey(exposedGeneralParameterName);
            PlayerPrefs.DeleteKey(exposedSfxParameterName);
            PlayerPrefs.DeleteKey(exposedMusicVolumeParameterName);
            SetupInitialSliderValue(exposedGeneralParameterName, ref generalVolumeSlider);
            SetupInitialSliderValue(exposedSfxParameterName, ref sfxVolumeSlider);
            SetupInitialSliderValue(exposedMusicVolumeParameterName, ref musicVolumeSlider);
        }
    }
}
