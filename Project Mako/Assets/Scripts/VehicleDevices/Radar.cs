using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace Mako.VehicleDevices
{
    public class Radar : MonoBehaviour
    {
        [SerializeField] private Camera radarCamera;
        [SerializeField] private TextMeshProUGUI jammedText;
        public void JamRadar()
        {
            radarCamera.gameObject.SetActive(false);
            jammedText.gameObject.SetActive(true);
        }

        public void RestoreRadar()
        {
            radarCamera.gameObject.SetActive(true);
            jammedText.gameObject.SetActive(false);
        }
    }
}
