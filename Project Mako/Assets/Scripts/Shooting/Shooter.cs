using System;
using Mako.Input;
using Mako.State;
using Mako.VehicleDevices;
using UnityEngine;
namespace Mako.Shooting
{
    public enum WeaponType
    {
        PRIMARY,
        SECONDARY
    }
    [RequireComponent(typeof(ProjectilesPool))]
    public class Shooter : MonoBehaviour
    {
        private bool inOverheat = false;
        private bool canShoot = false;
        private float cooldownTimer;
        private Vector3 mouseWorldPosition;
        private Vector3 aimDir = Vector3.zero;
        private Ray ray;
        private AudioSource audioSource;
        private PlayerInputActions playerInputActions;
        private InputManager _inputManager;
        private CanonBaseRotator canonBaseRotator;
        private ProjectilesPool projectilesPool;
        private GameManager gameManager;
        private ParticleSystem _flash;
        [SerializeField] private float cooldown;
        [SerializeField] private float overheatThreshold;
        [SerializeField] private float overheatPerShot;
        [SerializeField] private float coolMultiplier;
        [SerializeField] private GameObject projectile;
        [SerializeField] private Transform spawnPosition;
        [SerializeField] private LayerMask aimColliderLayerMask;
        [field: SerializeField] public WeaponType WeaponType { get; private set; } = WeaponType.PRIMARY;
        public float currentOverheat = 0;
        public Action OnOverheatChanged;
        private void Awake()
        {
            spawnPosition = GameObject.FindGameObjectWithTag("Projectile spawn pos").transform;
            audioSource = GetComponent<AudioSource>();
            canonBaseRotator = GetComponentInParent<CanonBaseRotator>();
            projectilesPool = GetComponent<ProjectilesPool>();
            gameManager = FindObjectOfType<GameManager>();
            _flash = GetComponentInChildren<ParticleSystem>();
            //projectilesPool.projectilePrefab = projectile;
        }
        public void Initialize(InputManager inputManager)
        {
            _inputManager = inputManager;
        }
        void Start()
        {
            cooldownTimer = cooldown;
        }

        void Update()
        {
            ManageShootingCapability();
            float shootInputValue = WeaponType == WeaponType.PRIMARY ? _inputManager.Actions.Player.Shooting.ReadValue<float>() :
            _inputManager.Actions.Player.SecondaryShooting.ReadValue<float>();

            if (shootInputValue == 1)
            {
                mouseWorldPosition = Vector3.zero;
                Ray ray = Camera.main.ScreenPointToRay(_inputManager.Actions.Player.Aiming.ReadValue<Vector2>());
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, aimColliderLayerMask))
                {
                    int layerNR = hit.transform.gameObject.layer;
                    bool aimingTooClose = layerNR == 3 || layerNR == 9;
                    if (aimingTooClose)
                    {
                        Shoot(canonBaseRotator.transform.forward, true);
                    }
                    else
                    {
                        mouseWorldPosition = hit.point;
                        Shoot(mouseWorldPosition, false); // Shoot at aim point
                    }
                }
            }
        }
        public float GetOverheatPercent()
        {
            return (float)currentOverheat / overheatThreshold;
        }
        private void ManageShootingCapability()
        {
            if (gameManager.GameIsPaused)
            {
                canShoot = false;
                return;
            }
            float overheatCooldownMultiplier;

            if (inOverheat && currentOverheat <= 0)
            {
                inOverheat = false;
            }
            currentOverheat -= Time.deltaTime * coolMultiplier;
            if (currentOverheat <= 0)
                currentOverheat = 0;
            if (currentOverheat >= overheatThreshold)
            {
                currentOverheat = overheatThreshold;
                inOverheat = true;
            }
            if (inOverheat)
            {
                overheatCooldownMultiplier = coolMultiplier;
                if (!audioSource.isPlaying)
                    audioSource.Play();
            }
            else
            {
                overheatCooldownMultiplier = coolMultiplier;
                audioSource.Stop();
            }

            if (cooldownTimer > 0 || currentOverheat >= overheatThreshold || inOverheat)
            {
                cooldownTimer -= Time.deltaTime;
                canShoot = false;
            }
            if (cooldownTimer <= 0 && currentOverheat < overheatThreshold && !inOverheat)
            {
                cooldownTimer = 0f;
                canShoot = true;
            }
            OnOverheatChanged?.Invoke();
        }

        private void Shoot(Vector3 directionOrTarget, bool isDirection)
        {
            ManageShootingCapability();
            if (!canShoot) return;

            var spawnedProjectile = projectilesPool.GetPooledProjectiles();
            if (spawnedProjectile)
            {
                spawnedProjectile.transform.position = spawnPosition.position;

                Vector3 shootDirection;
                if (isDirection)
                {
                    shootDirection = directionOrTarget;
                    spawnedProjectile.transform.rotation = Quaternion.LookRotation(shootDirection);
                }
                else
                {
                    shootDirection = (directionOrTarget - spawnPosition.position).normalized;
                    spawnedProjectile.transform.rotation = Quaternion.LookRotation(shootDirection);
                }
                _flash.Play();
                spawnedProjectile.SetActive(true);
                spawnedProjectile.GetComponentInChildren<Projectile>().OnShot(shootDirection);

                currentOverheat += overheatPerShot;
                canShoot = false;
                cooldownTimer = cooldown;
            }
        }
        public bool GetOverhearStatus()
        {
            return inOverheat;
        }
    }
}
