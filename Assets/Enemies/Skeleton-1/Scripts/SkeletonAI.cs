using UnityEngine;
using UnityEngine.AI;

public class SkeletonAI : MonoBehaviour
{
  private NavMeshAgent agent;
  void Start()
  {
    agent = GetComponent<NavMeshAgent>();
    agent.updateRotation = false;
    agent.updateUpAxis = false;
  }

  public void Update()
  {
    agent.SetDestination(FindObjectOfType<PlayerMovement>().transform.position);
  }
}
