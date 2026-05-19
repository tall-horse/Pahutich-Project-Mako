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

        void OnEnable()
        {
            SetupInitialSliderValue(exposedGeneralParameterName, ref generalVolumeSlider);
            SetupInitialSliderValue(exposedSfxParameterName, ref sfxVolumeSlider);
            SetupInitialSliderValue(exposedMusicVolumeParameterName, ref musicVolumeSlider);
        }

        private void SetupInitialSliderValue(string parameterName, ref Slider sliderToSetup)
        {
            float sliderStartValue = 0f;
            mixer.GetFloat(parameterName, out sliderStartValue);
            sliderStartValue = Mathf.Exp(sliderStartValue / 20);
            sliderToSetup.value = sliderStartValue;
        }

        public void ConfigureGeneralVolume()
        {
            mixer.SetFloat(exposedGeneralParameterName, Mathf.Log(generalVolumeSlider.value) * 20f);
        }
        public void ConfigureSFXVolume()
        {
            mixer.SetFloat(exposedSfxParameterName, Mathf.Log(sfxVolumeSlider.value) * 20f);
        }
        public void ConfigureMusicVolume()
        {
            mixer.SetFloat(exposedMusicVolumeParameterName, Mathf.Log(musicVolumeSlider.value) * 20f);
        }
    }
}
