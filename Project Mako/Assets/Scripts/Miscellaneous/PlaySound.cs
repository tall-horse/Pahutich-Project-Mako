using UnityEngine;

namespace Mako.Miscellaneous
{
    public class PlaySound : MonoBehaviour
    {
        private AudioSource audioSource;
        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
        }
        public void Play()
        {
            audioSource.Play();
        }
    }
}