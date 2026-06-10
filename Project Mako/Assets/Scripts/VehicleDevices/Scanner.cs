using Mako.Input;
using TMPro;
using UnityEngine;

namespace Mako.VehicleDevices
{
    public class Scanner : MonoBehaviour
    {
        private Vector3 mouseWorldPosition;
        [SerializeField] private LayerMask aimColliderLayerMask;
        [SerializeField] private Mako.UI.HealthBar healthBarOfScannedEnemy;
        [SerializeField] private TextMeshProUGUI enemyName;
        [SerializeField] private GameObject scannerPanel;
        private InputManager _inputManager;
        [field: SerializeField] public Health.BasicHealth ScannedEnemy { get; private set; }
        private void Awake()
        {
            scannerPanel.SetActive(false);
        }
        public void Initialize(InputManager inputManager)
        {
            _inputManager = inputManager;
        }

        // Update is called once per frame
        void Update()
        {
            mouseWorldPosition = Vector3.zero;
            Ray ray = Camera.main.ScreenPointToRay(_inputManager.Actions.Player.Aiming.ReadValue<Vector2>());
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 999f, aimColliderLayerMask))
            {
                ScannedEnemy = hit.collider.gameObject.GetComponentInParent<Health.BasicHealth>();
                if (hit.collider.gameObject.GetComponentInParent<Health.BasicHealth>() != null)
                {
                    enemyName.text = ScannedEnemy.GetHealthSystem().GetHolder();
                    healthBarOfScannedEnemy.ReconfigureHealthHolder(ScannedEnemy);
                    scannerPanel.SetActive(true);
                }
                else
                {
                    scannerPanel.SetActive(value: false);
                }
            }
        }
    }
}