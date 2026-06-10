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
        private bool _inOverheat = false;
        private bool _canShoot = false;
        private float _cooldownTimer;
        private Vector3 _mouseWorldPosition;
        private AudioSource _audioSource;
        private InputManager _inputManager;
        private CanonBaseRotator _canonBaseRotator;
        private ProjectilesPool _projectilesPool;
        private GameManager _gameManager;
        private ParticleSystem _flash;
        [SerializeField] private float cooldown;
        [SerializeField] private float overheatThreshold;
        [SerializeField] private float overheatPerShot;
        [SerializeField] private float coolMultiplier;
        [SerializeField] private GameObject projectile;
        [SerializeField] private Transform _projectileSpawnPosition;
        [SerializeField] private LayerMask aimColliderLayerMask;
        [field: SerializeField] public WeaponType WeaponType { get; private set; } = WeaponType.PRIMARY;
        public float currentOverheat = 0;
        public Action OnOverheatChanged;
        public void Initialize(InputManager inputManager, GameManager gameManager,
        CanonBaseRotator canonBaseRotator, Transform projectileSpawnPosition, ProjectilesPool projectilesPool,
        AudioSource audioSource, ParticleSystem flash)
        {
            _inputManager = inputManager;
            _gameManager = gameManager;
            _canonBaseRotator = canonBaseRotator;
            _projectileSpawnPosition = projectileSpawnPosition;
            _projectilesPool = projectilesPool;
            _audioSource = audioSource;
            _flash = flash;
        }
        void Start()
        {
            _cooldownTimer = cooldown;
        }

        void Update()
        {
            ManageShootingCapability();
            float shootInputValue = WeaponType == WeaponType.PRIMARY ? _inputManager.Actions.Player.Shooting.ReadValue<float>() :
            _inputManager.Actions.Player.SecondaryShooting.ReadValue<float>();

            if (shootInputValue == 1)
            {
                _mouseWorldPosition = Vector3.zero;
                Ray ray = Camera.main.ScreenPointToRay(_inputManager.Actions.Player.Aiming.ReadValue<Vector2>());
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, aimColliderLayerMask))
                {
                    int layerNR = hit.transform.gameObject.layer;
                    bool aimingTooClose = layerNR == 3 || layerNR == 9;
                    if (aimingTooClose)
                    {
                        Shoot(_canonBaseRotator.transform.forward, true);
                    }
                    else
                    {
                        _mouseWorldPosition = hit.point;
                        Shoot(_mouseWorldPosition, false); // Shoot at aim point
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
            if (_gameManager.GameIsPaused)
            {
                _canShoot = false;
                return;
            }
            float overheatCooldownMultiplier;

            if (_inOverheat && currentOverheat <= 0)
            {
                _inOverheat = false;
            }
            currentOverheat -= Time.deltaTime * coolMultiplier;
            if (currentOverheat <= 0)
                currentOverheat = 0;
            if (currentOverheat >= overheatThreshold)
            {
                currentOverheat = overheatThreshold;
                _inOverheat = true;
            }
            if (_inOverheat)
            {
                overheatCooldownMultiplier = coolMultiplier;
                if (!_audioSource.isPlaying)
                    _audioSource.Play();
            }
            else
            {
                overheatCooldownMultiplier = coolMultiplier;
                _audioSource.Stop();
            }

            if (_cooldownTimer > 0 || currentOverheat >= overheatThreshold || _inOverheat)
            {
                _cooldownTimer -= Time.deltaTime;
                _canShoot = false;
            }
            if (_cooldownTimer <= 0 && currentOverheat < overheatThreshold && !_inOverheat)
            {
                _cooldownTimer = 0f;
                _canShoot = true;
            }
            OnOverheatChanged?.Invoke();
        }

        private void Shoot(Vector3 directionOrTarget, bool isDirection)
        {
            ManageShootingCapability();
            if (!_canShoot) return;

            var spawnedProjectile = _projectilesPool.GetPooledProjectiles();
            if (spawnedProjectile)
            {
                spawnedProjectile.transform.position = _projectileSpawnPosition.position;

                Vector3 shootDirection;
                if (isDirection)
                {
                    shootDirection = directionOrTarget;
                    spawnedProjectile.transform.rotation = Quaternion.LookRotation(shootDirection);
                }
                else
                {
                    shootDirection = (directionOrTarget - _projectileSpawnPosition.position).normalized;
                    spawnedProjectile.transform.rotation = Quaternion.LookRotation(shootDirection);
                }
                _flash.Play();
                spawnedProjectile.SetActive(true);
                spawnedProjectile.GetComponentInChildren<Projectile>().OnShot(shootDirection);

                currentOverheat += overheatPerShot;
                _canShoot = false;
                _cooldownTimer = cooldown;
            }
        }
        public bool GetOverhearStatus()
        {
            return _inOverheat;
        }
    }
}
