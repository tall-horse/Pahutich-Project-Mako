using TMPro;
using UnityEngine;

public class Scanner : MonoBehaviour
{
    private Vector3 mouseWorldPosition;
    [SerializeField] private LayerMask aimColliderLayerMask;
    [SerializeField] private HealthBar healthBarOfScannedEnemy;
    [SerializeField] private TextMeshProUGUI enemyName;
    [SerializeField] private GameObject scannerPanel;
    [field: SerializeField] public Health ScannedEnemy { get; private set; }
    private void Awake()
    {
        scannerPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        mouseWorldPosition = Vector3.zero;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 999f, aimColliderLayerMask))
        {
            ScannedEnemy = hit.collider.gameObject.GetComponentInParent<Health>();
            if (hit.collider.gameObject.GetComponentInParent<Health>() != null)
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
