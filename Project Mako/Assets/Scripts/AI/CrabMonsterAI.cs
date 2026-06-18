using UnityEngine;
using UnityEngine.AI;

namespace Mako.AI
{
    public class CrabMonsterAI : MonoBehaviour
    {
        private NavMeshAgent agent;
        private Animator anim;
        private State currentState;
        [SerializeField] private string _currentStatename;
        public Transform player;
        // Start is called before the first frame update
        void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            anim = GetComponent<Animator>();
            currentState = new Idle(gameObject, agent, anim, player);
            _currentStatename = currentState.name.ToString();
        }

        // Update is called once per frame
        void Update()
        {
            currentState = currentState.Process();
            _currentStatename = currentState.name.ToString();
        }
        public void SetPlayerTarget()
        {
            currentState.heardPlayer = true;
        }
        public void SetDeadState()
        {
            var nextState = new Dead(gameObject, agent, anim, player);
            currentState = nextState;
        }
    }
}
