using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private float drag;
    private float angularDrag;
    private float timer;
    private Rigidbody projectileRigidbody;
    private Collider hitBox;
    private MeshRenderer meshRenderer;
    [SerializeField] private bool debuggingAiming = false;
    [SerializeField] private float speed;
    [SerializeField] private float timeToDisappear = 3f;
    [SerializeField] private int damageToDeal;
    [SerializeField] private GameObject collisonDebugPrefab;
    private void Awake()
    {
        projectileRigidbody = GetComponent<Rigidbody>();
        hitBox = GetComponentInChildren<Collider>();
        meshRenderer = GetComponentInChildren<MeshRenderer>();
        drag = projectileRigidbody.drag;
        angularDrag = projectileRigidbody.angularDrag;
    }

    private void OnEnable()
    {
        meshRenderer.enabled = true;
        hitBox.enabled = true;
        projectileRigidbody.angularDrag = angularDrag;
        projectileRigidbody.drag = drag;
        timer = 0f;
    }

    private void OnDisable()
    {
        meshRenderer.enabled = false;
        hitBox.enabled = false;
        projectileRigidbody.angularDrag = 0;
        projectileRigidbody.drag = 0;
        projectileRigidbody.velocity = Vector3.zero;
        projectileRigidbody.angularVelocity = Vector3.zero;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other == null)
            return;
        Health health = other.gameObject.GetComponent<Health>();
        Shields shields = other.gameObject.GetComponent<Shields>();
        if (shields != null)
        {
            shields.OnHitReceived(damageToDeal);
        }
        else if (health != null)
        {
            health.GetHealthSystem().Damage(damageToDeal);
        }
        StartCoroutine(SelfDestroy());
        if (debuggingAiming)
        {
            Instantiate(collisonDebugPrefab, transform.position, Quaternion.identity);
        }
    }

    public void OnShot(Vector3 direction)
    {
        projectileRigidbody.AddForce(direction * speed, ForceMode.Impulse);
    }

    private IEnumerator SelfDestroy()
    {
        PrepareToDisableProjectile();
        yield return new WaitForSeconds(1);
        //workaround for bullet being not a single object
        if (transform.parent != null)
        {
            transform.parent.gameObject.SetActive(false);
        }
        gameObject.SetActive(false);
    }

    private void PrepareToDisableProjectile()
    {
        projectileRigidbody.angularDrag = 0;
        projectileRigidbody.drag = 0;
        projectileRigidbody.velocity = Vector3.zero;
        projectileRigidbody.angularVelocity = Vector3.zero;
        meshRenderer.enabled = false;
        hitBox.enabled = false;
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer > timeToDisappear)
        {
            gameObject.SetActive(false);
        }
    }
}
