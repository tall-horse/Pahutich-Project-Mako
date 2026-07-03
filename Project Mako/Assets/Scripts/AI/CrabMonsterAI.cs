using Mako.Movement;
using Mako.Shooting;
using UnityEngine;
using UnityEngine.AI;

namespace Mako.AI
{
    public class CrabMonsterAI : MonoBehaviour
    {
        private NavMeshAgent _agent;
        private Animator _animator;
        private State _currentState;
        [SerializeField] private string _currentStatename;
        public Transform player;
        private EnemyManager _enemyManager;
        private MeleeAttack _meleeAttack;

        void Start()
        {
            _agent = GetComponent<NavMeshAgent>();
            _animator = GetComponent<Animator>();
            _meleeAttack = GetComponent<MeleeAttack>();
            _currentState = new Idle(gameObject, _agent, _animator, player);
            _currentStatename = _currentState.name.ToString();
            _enemyManager = FindObjectOfType<EnemyManager>();
            _enemyManager.Register(this);
            _meleeAttack.Initialize(player);
        }

        void Update()
        {
            _currentState = _currentState.Process();
            _currentStatename = _currentState.name.ToString();
        }
        public void SetPlayerTarget()
        {
            _currentState.heardPlayer = true;
        }
        public void SetDeadState()
        {
            var nextState = new Dead(gameObject, _agent, _animator, player);
            _currentState = nextState;
        }
        public EnemyManager GetEnemyManager()
        {
            return _enemyManager;
        }
    }
}
