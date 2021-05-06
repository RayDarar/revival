using UnityEngine;
using UnityEngine.AI;

public class SkeletonController : MonoBehaviour {
  private readonly float lookRadius = 8f;
  private NavMeshAgent agent;
  void Start() {
    agent = GetComponent<NavMeshAgent>();
    agent.updateRotation = false;
    agent.updateUpAxis = false;
  }

  public void OnDrawGizmosSelected() {
    Gizmos.color = Color.red;
    Gizmos.DrawWireSphere(transform.position, lookRadius);
  }

  public void Update() {
    agent.SetDestination(FindObjectOfType<PlayerMovement>().transform.position);
  }
}
