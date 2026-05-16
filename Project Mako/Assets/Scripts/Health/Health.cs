using System.Collections;
using UnityEngine;

namespace Mako.Health
{

  public abstract class Health : MonoBehaviour
  {
    protected Collider hitBox;
    protected AudioSource audioSource;
    protected MeshRenderer meshRenderer;
    protected HealthSystem healthSystem;
    [SerializeField] protected int health;
    [SerializeField] protected string healthHolderName;
    [SerializeField] protected AudioSource respectiveAudioImpact;
    protected virtual void Awake()
    {
      SetupHealthObject();
    }

    protected virtual void OnEnable()
    {
      SubscribeEvents();
    }

    protected virtual void SubscribeEvents()
    {
      if (healthSystem == null)
        SetupHealthObject();
      healthSystem.OnHealthChanged += StartDestructionProcess;
      healthSystem.OnDead += StartCorSelfDestroy;
    }

    protected virtual void OnDisable()
    {
      UnsubscribeEvents();
    }

    protected virtual void UnsubscribeEvents()
    {
      healthSystem.OnHealthChanged -= StartDestructionProcess;
      healthSystem.OnDead -= StartCorSelfDestroy;
    }

    protected void SetupHealthObject()
    {
      healthSystem = new HealthSystem(health, healthHolderName);

      hitBox = GetComponentInChildren<Collider>();
      audioSource = GetComponentInChildren<AudioSource>();
      meshRenderer = GetComponentInChildren<MeshRenderer>();
    }

    public HealthSystem GetHealthSystem()
    {
      return healthSystem;
    }
    protected void StartDestructionProcess(HealthSystem hs)
    {
      if (hs.GetHealth() <= 0)
        StartCoroutine(SelfDestroy());
    }
    public void PlayImpactSound()
    {
      respectiveAudioImpact.Play();
    }
    protected void StartCorSelfDestroy()
    {
      StartCoroutine(SelfDestroy());
    }
    protected virtual IEnumerator SelfDestroy()
    {
      audioSource.Play();
      meshRenderer.enabled = false;
      hitBox.enabled = false;
      foreach (Transform item in transform)
      {
        Destroy(item.gameObject);
      }
      yield return new WaitForSeconds(1f);
      Destroy(gameObject);
    }
  }
}
