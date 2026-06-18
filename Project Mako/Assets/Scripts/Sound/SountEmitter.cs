using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mako
{
    public class SountEmitter : MonoBehaviour
    {
        private AudioSource _audioSource;
        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        public void PlaySound()
        {
            _audioSource.Play();
        }
    }
}
