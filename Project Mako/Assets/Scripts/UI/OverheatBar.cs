using Mako.Movement;
using Mako.Shooting;
using UnityEngine;
using UnityEngine.UI;

namespace Mako.UI
{
    public class OverheatBar : MonoBehaviour
    {
        public Shooter overheatableWeapon;
        [SerializeField] private Image overheatBarImage;
        [SerializeField] private RawImage iconColor;
        private PlayerController playerController;
        void Awake()
        {
            playerController = FindObjectOfType<PlayerController>();
        }
        // Start is called before the first frame update
        void OnEnable()
        {
            AssignMainWeapon();
        }

        private void AssignMainWeapon()
        {
            var candidateWeapon = playerController.GetComponentInChildren<Shooter>();
            if (candidateWeapon.WeaponType == WeaponType.PRIMARY)
            {
                overheatableWeapon = candidateWeapon;
                overheatableWeapon.OnOverheatChanged += UpdateOverheatBarValue;
            }
        }

        void OnDisable()
        {
            overheatableWeapon.OnOverheatChanged -= UpdateOverheatBarValue;
            overheatableWeapon = null;
        }

        private void UpdateOverheatBarValue()
        {
            overheatBarImage.fillAmount = overheatableWeapon.GetOverheatPercent();
            iconColor.color = overheatableWeapon.GetOverhearStatus() ?
            iconColor.color = Color.red : iconColor.color = Color.white;
        }
    }
}
