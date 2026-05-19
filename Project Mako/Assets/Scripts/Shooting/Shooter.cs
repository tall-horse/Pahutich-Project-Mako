using System;
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
        private CanonBaseRotator canonBaseRotator;
        private ProjectilesPool projectilesPool;
        private GameManager gameManager;
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
            playerInputActions = new PlayerInputActions();
            canonBaseRotator = GetComponent<CanonBaseRotator>();
            projectilesPool = GetComponent<ProjectilesPool>();
            gameManager = FindObjectOfType<GameManager>();
            playerInputActions.Player.Enable();
            //projectilesPool.projectilePrefab = projectile;
        }

        void Start()
        {
            cooldownTimer = cooldown;
        }

        void Update()
        {
            ManageShootingCapability();
            float shootInputValue = WeaponType == WeaponType.PRIMARY ? playerInputActions.Player.Shooting.ReadValue<float>() :
            playerInputActions.Player.SecondaryShooting.ReadValue<float>();

            if (shootInputValue == 1)
            {
                mouseWorldPosition = Vector3.zero;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 999f, aimColliderLayerMask))
                {
                    mouseWorldPosition = hit.point;
                    Shoot(mouseWorldPosition);
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

        private void Shoot(Vector3 mousePosition)
        {
            ManageShootingCapability();
            if (!canShoot)
                return;
            aimDir = ((mousePosition - spawnPosition.gameObject.transform.position) + new Vector3(0, 0.5f, 0)).normalized;
            var spawnedProjectile = projectilesPool.GetPooledProjectiles();
            if (spawnedProjectile)
            {
                spawnedProjectile.transform.position = spawnPosition.position;
                Vector3 horizontalAimDir = new Vector3(aimDir.x, 0, aimDir.z);
                spawnedProjectile.transform.rotation = Quaternion.LookRotation(horizontalAimDir);
                spawnedProjectile.SetActive(true);
                spawnedProjectile.GetComponentInChildren<Projectile>().OnShot(aimDir);
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
