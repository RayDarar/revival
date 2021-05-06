using UnityEngine;
using UnityEngine.AI;

public class SkeletonController : MonoBehaviour {
  private readonly float lookRadius = 6f;
  private NavMeshAgent agent;
  private PlayerController player;

  void Start() {
    agent = GetComponent<NavMeshAgent>();
    agent.updateRotation = false;
    agent.updateUpAxis = false;

    player = PlayerManager.Instance.player;
  }

  public void OnDrawGizmosSelected() {
    Gizmos.color = Color.red;
    Gizmos.DrawWireSphere(transform.position, lookRadius);
  }

  public void Update() {
    float dist = Vector2.Distance(transform.position, player.transform.position);

    if (dist < lookRadius) {
      agent.SetDestination(player.transform.position);
    }
  }
}
