using System.Collections;
using Mako.VehicleDevices;
using UnityEngine;

namespace Mako.Shooting
{
  public class Projectile : MonoBehaviour
  {
    private float drag;
    private float angularDrag;
    private float timer;
    protected Rigidbody projectileRigidbody;
    private Collider hitBox;
    private MeshRenderer meshRenderer;
    private TrailRenderer trailRenderer;
    [SerializeField] private bool debuggingAiming = false;
    [SerializeField] protected float speed;
    [SerializeField] private float timeToDisappear = 3f;
    [SerializeField] private int damageToDeal;
    [SerializeField] private GameObject collisonDebugPrefab;
    protected virtual void Awake()
    {
      projectileRigidbody = GetComponentInChildren<Rigidbody>();
      hitBox = GetComponentInChildren<Collider>();
      meshRenderer = GetComponentInChildren<MeshRenderer>();
      trailRenderer = GetComponentInChildren<TrailRenderer>();
      drag = projectileRigidbody.drag;
      angularDrag = projectileRigidbody.angularDrag;
    }

    protected virtual void OnEnable()
    {
      meshRenderer.enabled = true;
      hitBox.enabled = true;
      projectileRigidbody.angularDrag = angularDrag;
      projectileRigidbody.drag = drag;
      trailRenderer.emitting = true;
      timer = 0f;
    }

    protected virtual void OnDisable()
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
      Health.BasicHealth health = other.gameObject.GetComponent<Health.BasicHealth>();
      Shields shields = other.gameObject.GetComponent<Shields>();
      if (shields != null)
      {
        if (shields.GetShieldCapacity() >= 0)
          shields.OnHitReceived(damageToDeal);
      }
      if (health != null && shields == null || shields != null && shields.GetShieldCapacity() <= 0)
      {
        health.GetHealthSystem().Damage(damageToDeal);
        health.PlayImpactSound();
      }
      StartCoroutine(SelfDestroy());
      if (debuggingAiming)
      {
        Instantiate(collisonDebugPrefab, transform.position, Quaternion.identity);
      }
    }
    public virtual void OnShot(Vector3 direction)
    {
      projectileRigidbody.AddForce(direction * speed, ForceMode.Impulse);
    }

    private IEnumerator SelfDestroy()
    {
      PrepareToDisableProjectile();
      yield return new WaitForSeconds(1);
      //a workaround for bullet being not a single object
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
      trailRenderer.emitting = false;
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
}
