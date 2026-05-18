using System.Collections;
using UnityEngine;

namespace Mako.Health
{

  public abstract class BasicHealth : MonoBehaviour
  {
    protected Collider hitBox;
    protected AudioSource audioSource;
    protected MeshRenderer meshRenderer;
    protected HealthSystem healthSystem;
    protected Animator animator;
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

    public void SetupHealthObject()
    {
      healthSystem = new HealthSystem(health, healthHolderName);

      hitBox = GetComponentInChildren<Collider>();
      audioSource = GetComponentInChildren<AudioSource>();
      meshRenderer = GetComponentInChildren<MeshRenderer>();
      animator = GetComponent<Animator>();
    }

    public HealthSystem GetHealthSystem()
    {
      return healthSystem;
    }
    protected virtual void StartDestructionProcess(HealthSystem hs)
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
    protected abstract IEnumerator SelfDestroy();
  }

}
