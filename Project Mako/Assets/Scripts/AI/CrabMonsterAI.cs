using UnityEngine;
using UnityEngine.AI;

namespace Mako.AI
{
  public class CrabMonsterAI : MonoBehaviour
  {
    private NavMeshAgent agent;
    private Animator anim;
    private State currentState;
    public Transform player;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        currentState = new Idle(gameObject, agent, anim, player);
    }

    // Update is called once per frame
    void Update()
    {
      currentState = currentState.Process();
    }
    public void GoOnSound()
    {
      currentState.heardPlayer = true;
    }
  }
}
