using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mako.Shooting
{
    [RequireComponent(typeof(ProjectilesPool))]
    public class Turret : MonoBehaviour
    {
        private const string PLAYERTAG = "Player";

        [Header("Attributes")]
        [SerializeField] private float range = 15f;
        [SerializeField] private float rangeExtensinIndex = 1.5f;
        private float extendedRange;
        private float initialRange;
        [SerializeField] private float fireRate = 1f;

        [Header("Unity setup fields")]

        [SerializeField] private float turnSpeed = 10f;
        private float fireCountdown = 0f;
        private float distanceToPlayer;
        private GameObject player;
        public bool playerInRange = false;
        public bool dead = false;
        [SerializeField] private List<MeshRenderer> _turretVisuals;
        [SerializeField] private Transform firePoint;
        [SerializeField] private Transform firePoint2;
        private ProjectilesPool projectilesPool;
        private void Awake()
        {
            player = GameObject.FindGameObjectWithTag(PLAYERTAG);
            projectilesPool = GetComponent<ProjectilesPool>();
            initialRange = range;
        }
        // Start is called before the first frame update
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

        // Update is called once per frame
        void Update()
        {
            if (dead) return;
            if (player == null)
                return;

            if (playerInRange)
            {
                Vector3 dir = player.transform.position - transform.position;
                Quaternion lookRotation = Quaternion.LookRotation(dir);
                Vector3 rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
                transform.rotation = Quaternion.Euler(0, rotation.y, 0);

                if (fireCountdown <= 0f)
                {
                    Shoot();
                    fireCountdown = 1f / fireRate;

                }
                fireCountdown -= Time.deltaTime;
            }
        }

        private void Shoot()
        {
            if (dead) return;
            var spawnedProjectile = projectilesPool.GetPooledProjectiles();
            if (spawnedProjectile)
            {
                spawnedProjectile.transform.position = firePoint.position;
                spawnedProjectile.transform.rotation = Quaternion.identity;
                spawnedProjectile.SetActive(true);
                //workaround for bullet having children
                if (spawnedProjectile.transform.childCount > 0)
                {
                    foreach (Transform child in spawnedProjectile.transform)
                    {
                        child.gameObject.SetActive(true);
                    }
                }
                spawnedProjectile.GetComponent<Projectile>().OnShot(player.transform.position - transform.position);
            }
            spawnedProjectile = projectilesPool.GetPooledProjectiles();
            if (spawnedProjectile)
            {
                spawnedProjectile.transform.position = firePoint2.position;
                spawnedProjectile.transform.rotation = Quaternion.identity;
                spawnedProjectile.SetActive(true);
                //workaround for bullet having children
                if (spawnedProjectile.transform.childCount > 0)
                {
                    foreach (Transform child in spawnedProjectile.transform)
                    {
                        child.gameObject.SetActive(true);
                    }
                }
                spawnedProjectile.GetComponent<Projectile>().OnShot(player.transform.position - transform.position);
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

    }
}
