using UnityEngine;
using UnityEngine.AI;

public class GenericEnemyBuilder {
  private readonly GenericEnemyController controller;
  public GenericEnemyBuilder(GenericEnemyController controller) {
    this.controller = controller;
  }

  public GenericEnemyBuilder SetLookRadius(float value) {
    controller.lookRadius = value;
    return this;
  }

  public GenericEnemyBuilder SetHealth(float value) {
    controller.health = value;
    return this;
  }
}

public abstract class GenericEnemyController : MonoBehaviour {
  [HideInInspector]
  public NavMeshAgent agent;

  [HideInInspector]
  public PlayerController player;

  [HideInInspector]
  public float lookRadius;

  [HideInInspector]
  public float health;

  public virtual void Awake() {
    SetupEnemy(new GenericEnemyBuilder(this));
  }

  public virtual void Start() {
    agent = GetComponent<NavMeshAgent>();
    agent.updateRotation = false;
    agent.updateUpAxis = false;

    player = PlayerManager.Instance.player;
  }

  public void OnDrawGizmosSelected() {
    Gizmos.color = Color.red;
    Gizmos.DrawWireSphere(transform.position, lookRadius);
  }

  public abstract void SetupEnemy(GenericEnemyBuilder builder);

  public void MoveTowardsPlayerRadius() {
    float dist = Vector2.Distance(transform.position, player.transform.position);

    if (dist < lookRadius) {
      agent.SetDestination(player.transform.position);
    }
  }
}