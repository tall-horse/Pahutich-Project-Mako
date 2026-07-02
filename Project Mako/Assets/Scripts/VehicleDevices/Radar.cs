using UnityEngine;
using TMPro;
namespace Mako.VehicleDevices
{
    public class Radar : MonoBehaviour
    {
        [SerializeField] private Camera radarCamera;
        [SerializeField] private TextMeshProUGUI jammedText;
        private void Awake()
        {
            if (radarCamera == null)
                Debug.LogError("Radar camera not assigned");
            if (jammedText == null)
                Debug.LogError("JammedText not assigned");
        }
        public void JamRadar()
        {
            if (radarCamera == null || jammedText == null)
            {
                Debug.LogError("Radar was not jammed due to missing references in Radar");
                return;
            }
            radarCamera.gameObject.SetActive(false);
            jammedText.gameObject.SetActive(true);
        }

        public void RestoreRadar()
        {
            if (radarCamera == null)
            {
                return;
            }
            radarCamera.gameObject.SetActive(true);
            jammedText.gameObject.SetActive(false);
        }
    }
}
