using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mako.Shooting
{
    [RequireComponent(typeof(ProjectilesPool))]
    public class Turret : MonoBehaviour
    {
        public enum TurretState
        {
            Hidden,
            Transitioning,
            Out,
            Destroyed
        }
        [SerializeField] private TurretState _turretState = TurretState.Hidden;
        private const string PLAYERTAG = "Player";

        [Header("Attributes")]
        [SerializeField] private float range = 15f;
        [SerializeField] private float rangeExtensinIndex = 1.5f;
        private float extendedRange;
        private float initialRange;
        [SerializeField] private float fireRate = 1f;
        [SerializeField] private float _timeToHide = 3f;
        private float _hideTimer = 0f;

        [Header("Unity setup fields")]

        [SerializeField] private float turnSpeed = 10f;
        private float fireCountdown = 0f;
        private float distanceToPlayer;
        private GameObject player;
        public bool playerInRange = false;
        public bool dead = false;
        [SerializeField] private List<MeshRenderer> _turretVisuals;
        [SerializeField] private AudioSource[] _firePoints;
        [SerializeField] private ParticleSystem[] _gunParticles;
        private ProjectilesPool projectilesPool;
        [SerializeField] private Transform _gunsMesh;
        private Vector3 _shootingDirection;
        private TurretAnimationController _animationController;
        private Animator _animator;
        private void Awake()
        {
            player = GameObject.FindGameObjectWithTag(PLAYERTAG);
            projectilesPool = GetComponent<ProjectilesPool>();
            _animator = GetComponentInParent<Animator>();
            _animationController = GetComponentInParent<TurretAnimationController>();
            initialRange = range;
        }
        private void OnEnable()
        {
            _animationController.OnTurretStateChange += ChangeTurretState;
        }

        private void OnDisable()
        {
            _animationController.OnTurretStateChange -= ChangeTurretState;
        }

        void Start()
        {
            if (dead) return;
            InvokeRepeating("UpdateTarget", 0, 0.5f);
            extendedRange = range * rangeExtensinIndex;
        }
        private void UpdateTarget()
        {
            if (dead) return;
            if (player == null)
                return;
            distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
            if (distanceToPlayer <= range)
            {
                playerInRange = true;
            }
            else
            {
                playerInRange = false;
            }
        }

        void Update()
        {
            if (dead) return;
            if (player == null)
                return;

            if (playerInRange)
            {
                if (_turretState == TurretState.Hidden)
                {
                    if (_animator == null) Debug.Log("animator is null");
                    _animator.SetBool("Activate", true);
                    return;
                }
                if (_turretState == TurretState.Out)
                {
                    Vector3 dir = player.transform.position - transform.position;
                    Quaternion lookRotation = Quaternion.LookRotation(dir);
                    Vector3 rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
                    transform.rotation = Quaternion.Euler(0, rotation.y, 0);

                    _shootingDirection = player.transform.position - transform.position;
                    Vector3 euler = Quaternion.LookRotation(dir, Vector3.up).eulerAngles;
                    float pitch = Mathf.Clamp(Mathf.DeltaAngle(0f, euler.x), -90f, 40f);
                    _gunsMesh.transform.rotation = Quaternion.Euler(pitch, euler.y, euler.z);

                    if (fireCountdown <= 0f && _turretState == TurretState.Out)
                    {
                        Shoot();
                        fireCountdown = 1f / fireRate;

                    }
                    fireCountdown -= Time.deltaTime;
                    _hideTimer = 0f;
                }
            }
            else
            {
                if (_turretState == TurretState.Out)
                {
                    _hideTimer += Time.deltaTime;

                    if (_hideTimer >= _timeToHide)
                    {
                        _turretState = TurretState.Hidden;
                        _animator.enabled = true;
                        _animator.SetBool("Activate", false);
                    }
                }
            }
        }

        private void Shoot()
        {
            if (dead) return;

            for (int i = 0; i < _firePoints.Length; i++)
            {
                var spawnedProjectile = projectilesPool.GetPooledProjectiles();
                if (!spawnedProjectile) continue;
                spawnedProjectile.transform.position = _firePoints[i].transform.position;
                spawnedProjectile.transform.rotation = _gunsMesh.transform.rotation;
                spawnedProjectile.SetActive(true);
                spawnedProjectile.GetComponent<Projectile>().OnShot(_shootingDirection);

                var ps = _gunParticles[i];
                ps.transform.position = _firePoints[i].transform.position;
                ps.transform.rotation = Quaternion.LookRotation(_shootingDirection);
                _firePoints[i].Play();
                ps.Play();
            }
        }


        public void ExtendRange()
        {
            range = extendedRange;
        }

        public IEnumerable GetTurretVisuals()
        {
            return _turretVisuals;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, range);
        }
        private void ChangeTurretState(TurretState turretState, bool animatorEnabled)
        {
            _turretState = turretState;
            _animator.enabled = animatorEnabled;
        }
    }
}
