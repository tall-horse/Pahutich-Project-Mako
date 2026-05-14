using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mako
{
    public class CoroutinePerformer : MonoBehaviour
    {
        private void Awake() {
            DontDestroyOnLoad(gameObject);
        }
    }
}
